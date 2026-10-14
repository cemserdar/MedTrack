/**
 * MedTrack — Clinical Dashboard & Real-Time Management
 * Integrated with ASP.NET Core WebAPI & SignalR
 */

const API_BASE = '/api';
const HUB_URL = '/hubs/medical';

// State
const state = {
    token: localStorage.getItem('medtrack_token') || '',
    user: JSON.parse(localStorage.getItem('medtrack_user') || 'null'),
    patients: [],
    doctors: [],
    clinics: [],
    appointments: [],
    insuranceTypes: [],
    diagnosisCodes: [],
    labTests: [],
    prescriptions: [],
    signalRConnected: false,
    theme: localStorage.getItem('medtrack_theme') || 'dark',
    chartInstance: null,
    distChartInstance: null
};

// SignalR Connection
let hubConnection = null;

// Initialization
document.addEventListener('DOMContentLoaded', async () => {
    initTheme();
    setupNavigation();
    setupModals();
    setupSearch();

    // Auto-login or verify session
    await ensureAuthentication();

    // Initialize SignalR
    initSignalR();

    // Load Initial Clinical Data
    await loadInitialData();

    // Update KPI & Charts
    renderDashboard();
});

/* ==========================================================================
   Theme Management
   ========================================================================== */
function initTheme() {
    document.documentElement.setAttribute('data-theme', state.theme);
    const themeBtn = document.getElementById('themeToggleBtn');
    if (themeBtn) {
        themeBtn.addEventListener('click', () => {
            state.theme = state.theme === 'dark' ? 'light' : 'dark';
            document.documentElement.setAttribute('data-theme', state.theme);
            localStorage.setItem('medtrack_theme', state.theme);
            if (state.chartInstance) state.chartInstance.destroy();
            if (state.distChartInstance) state.distChartInstance.destroy();
            initCharts();
        });
    }
}

/* ==========================================================================
   Authentication & JWT Handling
   ========================================================================== */
async function ensureAuthentication() {
    if (!state.token) {
        // Automatically authenticate with default seeded Admin for immediate access
        try {
            const res = await fetch(`${API_BASE}/Auth/login`, {
                method: 'POST',
                headers: { 'Content-Type': 'application/json' },
                body: JSON.stringify({ username: 'admin', password: 'Admin@123456' })
            });
            if (res.ok) {
                const data = await res.json();
                state.token = data.token;
                state.user = { username: data.username, role: data.role };
                localStorage.setItem('medtrack_token', data.token);
                localStorage.setItem('medtrack_user', JSON.stringify(state.user));
            }
        } catch (err) {
            console.warn('Auto login skipped or error:', err);
        }
    }
    updateUserUI();
}

function updateUserUI() {
    const userDisplay = document.getElementById('currentUserName');
    const userRole = document.getElementById('currentUserRole');
    if (userDisplay && state.user) {
        userDisplay.textContent = state.user.username === 'admin' ? 'Yönetici (Admin)' : state.user.username;
        if (userRole) userRole.textContent = state.user.role || 'Sağlık Personeli';
    }
}

function getAuthHeaders() {
    const headers = { 'Content-Type': 'application/json' };
    if (state.token) {
        headers['Authorization'] = `Bearer ${state.token}`;
    }
    return headers;
}

/* ==========================================================================
   SignalR Real-Time WebSocket Connection
   ========================================================================== */
function initSignalR() {
    if (typeof signalR === 'undefined') {
        console.warn('SignalR client library not loaded yet');
        return;
    }

    const options = {
        accessTokenFactory: () => state.token
    };

    hubConnection = new signalR.HubConnectionBuilder()
        .withUrl(HUB_URL, options)
        .withAutomaticReconnect([0, 2000, 5000, 10000])
        .configureLogging(signalR.LogLevel.Information)
        .build();

    // Event: AppointmentCreatedBroadcast
    hubConnection.on('AppointmentCreatedBroadcast', (data) => {
        addLiveStreamEvent('Yeni Randevu Oluşturuldu', `Randevu ID: #${data.id.substring(0, 8)} - Tarih: ${new Date(data.date).toLocaleTimeString('tr-TR', { hour: '2-digit', minute: '2-digit' })}`, 'calendar');
        showToast('Randevu Bildirimi', 'Yeni bir randevu sisteme eklendi.', 'info');
        reloadAppointments();
    });

    // Event: AppointmentUpdated
    hubConnection.on('AppointmentUpdated', (data) => {
        addLiveStreamEvent('Randevu Güncellendi', `Randevu #${data.id.substring(0, 8)} detayları güncellendi.`, 'edit');
        showToast('Randevu Güncellendi', 'Randevu bilgileri güncellendi.', 'info');
        reloadAppointments();
    });

    // Event: AppointmentStatusUpdated
    hubConnection.on('AppointmentStatusUpdated', (data) => {
        const statusMap = { 'Scheduled': 'Bekliyor', 'Completed': 'Tamamlandı', 'Cancelled': 'İptal Edildi' };
        const statusText = statusMap[data.status] || data.status;
        addLiveStreamEvent('Randevu Durumu Değişti', `Randevu #${data.id.substring(0, 8)} durumu: ${statusText}`, 'check');
        showToast('Durum Güncellendi', `Randevu durumu "${statusText}" olarak değiştirildi.`, 'success');
        reloadAppointments();
    });

    // Event: UserConnected
    hubConnection.on('UserConnected', (username) => {
        addLiveStreamEvent('Kullanıcı Bağlandı', `${username} sisteme giriş yaptı.`, 'user');
    });

    // Event: UserDisconnected
    hubConnection.on('UserDisconnected', (username) => {
        addLiveStreamEvent('Kullanıcı Ayrıldı', `${username} sistemden ayrıldı.`, 'user');
    });

    // Connection lifecycle
    hubConnection.start()
        .then(() => {
            state.signalRConnected = true;
            updateSignalRUI(true);
            addLiveStreamEvent('SignalR Bağlantısı Kuruldu', 'MedTrack WebSocket MedicalHub gerçek zamanlı akış devrede.', 'check');
        })
        .catch(err => {
            console.error('SignalR Connection Error:', err);
            updateSignalRUI(false);
        });

    hubConnection.onreconnecting(() => {
        updateSignalRUI(false, 'Yeniden Bağlanıyor...');
    });

    hubConnection.onreconnected(() => {
        updateSignalRUI(true);
        addLiveStreamEvent('SignalR Yeniden Bağlandı', 'Gerçek zamanlı bağlantı tekrar sağlandı.', 'check');
    });

    hubConnection.onclose(() => {
        updateSignalRUI(false);
    });
}

function updateSignalRUI(connected, text) {
    const statusEl = document.getElementById('signalRStatusBadge');
    if (!statusEl) return;
    if (connected) {
        statusEl.className = 'signalr-status';
        statusEl.innerHTML = '<span class="pulse-dot"></span><span>Canlı Bağlantı</span>';
    } else {
        statusEl.className = 'signalr-status disconnected';
        statusEl.innerHTML = `<span class="pulse-dot"></span><span>${text || 'Bağlantı Kesildi'}</span>`;
    }
}

function addLiveStreamEvent(title, desc, iconType = 'info') {
    const streamBox = document.getElementById('liveStreamEvents');
    if (!streamBox) return;

    const timeStr = new Date().toLocaleTimeString('tr-TR', { hour: '2-digit', minute: '2-digit', second: '2-digit' });
    const item = document.createElement('div');
    item.className = 'stream-event-item';
    item.innerHTML = `
        <div class="stream-icon">
            <svg fill="none" stroke="currentColor" viewBox="0 0 24 24" width="16" height="16">
                <path stroke-linecap="round" stroke-linejoin="round" stroke-width="2" d="M13 10V3L4 14h7v7l9-11h-7z" />
            </svg>
        </div>
        <div class="stream-content">
            <div class="stream-header">
                <span class="stream-title">${escapeHtml(title)}</span>
                <span class="stream-time">${timeStr}</span>
            </div>
            <div class="stream-desc">${escapeHtml(desc)}</div>
        </div>
    `;
    streamBox.insertBefore(item, streamBox.firstChild);

    // Keep max 25 events
    if (streamBox.children.length > 25) {
        streamBox.removeChild(streamBox.lastChild);
    }
}

/* ==========================================================================
   Data Fetching & REST API Integration
   ========================================================================== */
async function loadInitialData() {
    try {
        const [patientsRes, doctorsRes, clinicsRes, appointmentsRes, insuranceRes, diagnosisRes] = await Promise.all([
            fetch(`${API_BASE}/Patients`, { headers: getAuthHeaders() }),
            fetch(`${API_BASE}/Doctors`, { headers: getAuthHeaders() }),
            fetch(`${API_BASE}/Clinics`, { headers: getAuthHeaders() }),
            fetch(`${API_BASE}/Appointments`, { headers: getAuthHeaders() }),
            fetch(`${API_BASE}/ReferenceData/insurance-types`, { headers: getAuthHeaders() }),
            fetch(`${API_BASE}/ReferenceData/diagnosis-codes`, { headers: getAuthHeaders() })
        ]);

        if (patientsRes.ok) state.patients = await patientsRes.json();
        if (doctorsRes.ok) state.doctors = await doctorsRes.json();
        if (clinicsRes.ok) state.clinics = await clinicsRes.json();
        if (appointmentsRes.ok) state.appointments = await appointmentsRes.json();
        if (insuranceRes.ok) state.insuranceTypes = await insuranceRes.json();
        if (diagnosisRes.ok) state.diagnosisCodes = await diagnosisRes.json();

        // Populate dropdowns in modals
        populateModalDropdowns();

    } catch (err) {
        console.error('Error fetching clinical data:', err);
        showToast('Veri Yükleme Uyarısı', 'Veriler yüklenirken bir problem oluştu.', 'info');
    }
}

async function reloadAppointments() {
    try {
        const res = await fetch(`${API_BASE}/Appointments`, { headers: getAuthHeaders() });
        if (res.ok) {
            state.appointments = await res.json();
            renderAppointments();
            renderDashboardKPIs();
        }
    } catch (err) {
        console.error('Error reloading appointments:', err);
    }
}

async function reloadPatients() {
    try {
        const res = await fetch(`${API_BASE}/Patients`, { headers: getAuthHeaders() });
        if (res.ok) {
            state.patients = await res.json();
            renderPatientsTable(state.patients);
            renderDashboardKPIs();
        }
    } catch (err) {
        console.error('Error reloading patients:', err);
    }
}

/* ==========================================================================
   UI Rendering: Dashboard, KPIs, Charts, Tables
   ========================================================================== */
function renderDashboard() {
    renderDashboardKPIs();
    renderTodayAppointments();
    renderPatientsTable(state.patients);
    renderDoctorsCards();
    renderClinicsList();
    renderAppointments();
    initCharts();
}

function renderDashboardKPIs() {
    const elPatients = document.getElementById('kpiTotalPatients');
    const elAppointments = document.getElementById('kpiTodayAppointments');
    const elDoctors = document.getElementById('kpiActiveDoctors');
    const elClinics = document.getElementById('kpiActiveClinics');

    if (elPatients) elPatients.textContent = state.patients.length;
    if (elAppointments) elAppointments.textContent = state.appointments.length;
    if (elDoctors) elDoctors.textContent = state.doctors.length;
    if (elClinics) elClinics.textContent = state.clinics.length;
}

function renderTodayAppointments() {
    const container = document.getElementById('todayAppointmentsList');
    if (!container) return;

    if (!state.appointments || state.appointments.length === 0) {
        container.innerHTML = `
            <div style="text-align: center; padding: 30px; color: var(--text-muted);">
                <p>Bugün için planlanmış randevu bulunmuyor.</p>
            </div>
        `;
        return;
    }

    container.innerHTML = state.appointments.slice(0, 6).map(app => {
        const patientName = app.patientName || (app.patient ? `${app.patient.firstName} ${app.patient.lastName}` : 'Kayıtlı Hasta');
        const doctorName = app.doctorName || (app.doctor ? app.doctor.fullName : 'Doktor');
        const clinicName = app.clinicName || (app.clinic ? app.clinic.name : 'Genel Poliklinik');
        const dateObj = new Date(app.appointmentDate);
        const timeStr = dateObj.toLocaleTimeString('tr-TR', { hour: '2-digit', minute: '2-digit' });
        const dateStr = dateObj.toLocaleDateString('tr-TR', { day: 'numeric', month: 'short' });

        return `
            <div class="appointment-item" id="app-item-${app.id}">
                <div class="app-time-col">
                    ${timeStr}
                    <span>${dateStr}</span>
                </div>
                <div class="app-patient-col">
                    <div class="patient-name-heading">${escapeHtml(patientName)}</div>
                    <div class="patient-detail-sub">
                        <span>🩺 ${escapeHtml(doctorName)}</span>
                        <span>•</span>
                        <span>🏥 ${escapeHtml(clinicName)}</span>
                    </div>
                </div>
                <div style="display: flex; align-items: center; gap: 8px;">
                    <span class="status-badge ${app.status}">${getStatusLabel(app.status)}</span>
                    <button class="status-select-btn" title="Durumu Güncelle" onclick="toggleAppointmentStatus('${app.id}', '${app.status}')">
                        <svg fill="none" stroke="currentColor" viewBox="0 0 24 24" width="18" height="18">
                            <path stroke-linecap="round" stroke-linejoin="round" stroke-width="2" d="M9 5l7 7-7 7" />
                        </svg>
                    </button>
                </div>
            </div>
        `;
    }).join('');
}

function renderAppointments() {
    const container = document.getElementById('allAppointmentsTableBody');
    if (!container) return;

    container.innerHTML = state.appointments.map(app => {
        const patientName = app.patientName || (app.patient ? `${app.patient.firstName} ${app.patient.lastName}` : 'Hasta');
        const doctorName = app.doctorName || (app.doctor ? app.doctor.fullName : 'Doktor');
        const clinicName = app.clinicName || (app.clinic ? app.clinic.name : '-');
        const dateObj = new Date(app.appointmentDate);
        const formattedDate = dateObj.toLocaleString('tr-TR', {
            day: '2-digit', month: '2-digit', year: 'numeric',
            hour: '2-digit', minute: '2-digit'
        });

        return `
            <tr>
                <td><strong>#${app.id.substring(0, 8)}</strong></td>
                <td>
                    <div style="font-weight: 600;">${escapeHtml(patientName)}</div>
                    <div style="font-size: 0.75rem; color: var(--text-muted);">${escapeHtml(app.notes || 'Şikayet girilmedi')}</div>
                </td>
                <td>${escapeHtml(doctorName)}</td>
                <td>${escapeHtml(clinicName)}</td>
                <td>${formattedDate}</td>
                <td>
                    <span class="status-badge ${app.status}">${getStatusLabel(app.status)}</span>
                </td>
                <td>
                    <div style="display: flex; gap: 6px;">
                        <button class="btn-secondary" style="padding: 4px 8px; font-size: 0.75rem;" onclick="changeStatusPrompt('${app.id}')">
                            Durum Değiştir
                        </button>
                    </div>
                </td>
            </tr>
        `;
    }).join('');
}

function renderPatientsTable(patients) {
    const tbody = document.getElementById('patientsTableBody');
    if (!tbody) return;

    if (!patients || patients.length === 0) {
        tbody.innerHTML = `<tr><td colspan="7" style="text-align:center; padding: 24px; color: var(--text-muted);">Hasta kaydı bulunamadı.</td></tr>`;
        return;
    }

    tbody.innerHTML = patients.map(p => {
        const initials = `${p.firstName ? p.firstName[0] : ''}${p.lastName ? p.lastName[0] : ''}`.toUpperCase();
        const birthStr = p.birthDate ? new Date(p.birthDate).toLocaleDateString('tr-TR') : '-';
        const genderLabel = p.gender === 'M' ? 'Erkek' : (p.gender === 'F' ? 'Kadın' : '-');

        return `
            <tr>
                <td>
                    <div class="patient-avatar-cell">
                        <div class="table-avatar">${initials}</div>
                        <div>
                            <div style="font-weight: 600;">${escapeHtml(p.firstName)} ${escapeHtml(p.lastName)}</div>
                            <div style="font-size: 0.75rem; color: var(--text-muted);">${escapeHtml(p.email || 'Email yok')}</div>
                        </div>
                    </div>
                </td>
                <td><code>${escapeHtml(p.nationalId || '-')}</code></td>
                <td>${genderLabel}</td>
                <td>${birthStr}</td>
                <td>${escapeHtml(p.phoneNumber || '-')}</td>
                <td>${escapeHtml(p.emergencyContactName || '-')}</td>
                <td>
                    <button class="btn-primary" style="padding: 4px 10px; font-size: 0.78rem;" onclick="openNewAppointmentModalForPatient('${p.id}')">
                        + Randevu Ver
                    </button>
                </td>
            </tr>
        `;
    }).join('');
}

function renderDoctorsCards() {
    const container = document.getElementById('doctorsGrid');
    if (!container) return;

    container.innerHTML = state.doctors.map(d => {
        const initials = d.fullName.split(' ').map(n => n[0]).join('').substring(0, 2);
        const clinic = state.clinics.find(c => c.id === d.clinicId);
        const clinicName = clinic ? clinic.name : 'Poliklinik';

        return `
            <div class="doctor-card">
                <div class="doctor-top-row">
                    <div class="doctor-avatar-lg">${initials}</div>
                    <div class="doctor-meta">
                        <h3>${escapeHtml(d.fullName)}</h3>
                        <span class="doctor-specialty-pill">${escapeHtml(d.specialty)}</span>
                    </div>
                </div>
                <p class="doctor-bio-text">${escapeHtml(d.biography || 'Doktor bilgisi mevcut.')}</p>
                <div class="doctor-info-list">
                    <div class="doctor-info-item">
                        <svg fill="none" stroke="currentColor" viewBox="0 0 24 24"><path stroke-linecap="round" stroke-linejoin="round" stroke-width="2" d="M19 21V5a2 2 0 00-2-2H7a2 2 0 00-2 2v16m14 0h2m-2 0h-5m-9 0H3m2 0h5M9 7h1m-1 4h1m4-4h1m-1 4h1m-5 10v-5a1 1 0 011-1h2a1 1 0 011 1v5m-4 0h4"/></svg>
                        <span>${escapeHtml(clinicName)}</span>
                    </div>
                    <div class="doctor-info-item">
                        <svg fill="none" stroke="currentColor" viewBox="0 0 24 24"><path stroke-linecap="round" stroke-linejoin="round" stroke-width="2" d="M3 5a2 2 0 012-2h3.28a1 1 0 01.948.684l1.498 4.493a1 1 0 01-.502 1.21l-2.257 1.13a11.042 11.042 0 005.516 5.516l1.13-2.257a1 1 0 011.21-.502l4.493 1.498a1 1 0 01.684.949V19a2 2 0 01-2 2h-1C9.716 21 3 14.284 3 6V5z"/></svg>
                        <span>${escapeHtml(d.phoneNumber || 'İletişim yok')}</span>
                    </div>
                    <div class="doctor-info-item">
                        <svg fill="none" stroke="currentColor" viewBox="0 0 24 24"><path stroke-linecap="round" stroke-linejoin="round" stroke-width="2" d="M9 12h6m-6 4h6m2 5H7a2 2 0 01-2-2V5a2 2 0 012-2h5.586a1 1 0 01.707.293l5.414 5.414a1 1 0 01.293.707V19a2 2 0 01-2 2z"/></svg>
                        <span>Diploma No: ${escapeHtml(d.licenseNumber || '-')}</span>
                    </div>
                </div>
                <button class="btn-primary" onclick="openNewAppointmentModalWithDoctor('${d.id}')">
                    Bu Doktordan Randevu Al
                </button>
            </div>
        `;
    }).join('');
}

function renderClinicsList() {
    const container = document.getElementById('clinicsGrid');
    if (!container) return;

    container.innerHTML = state.clinics.map(c => `
        <div class="panel-card" style="margin-bottom: 14px;">
            <div style="display: flex; justify-content: space-between; align-items: center;">
                <div>
                    <h4 style="font-size: 1.05rem; margin-bottom: 4px;">🏥 ${escapeHtml(c.name)}</h4>
                    <p style="color: var(--text-secondary); font-size: 0.85rem;">📍 ${escapeHtml(c.location)}</p>
                </div>
                <span class="status-badge Scheduled">Aktif Hizmet</span>
            </div>
        </div>
    `).join('');
}

/* ==========================================================================
   Interactive Chart.js Visualizations
   ========================================================================== */
function initCharts() {
    if (typeof Chart === 'undefined') return;

    const isDark = state.theme === 'dark';
    const textColor = isDark ? '#94a3b8' : '#475569';
    const gridColor = isDark ? 'rgba(255, 255, 255, 0.06)' : 'rgba(0, 0, 0, 0.06)';

    // Weekly Appointments Trend Chart
    const ctxTrend = document.getElementById('appointmentsTrendChart');
    if (ctxTrend) {
        state.chartInstance = new Chart(ctxTrend, {
            type: 'line',
            data: {
                labels: ['Pzt', 'Sal', 'Çar', 'Per', 'Cum', 'Cmt', 'Paz'],
                datasets: [{
                    label: 'Tamamlanan Randevular',
                    data: [14, 22, 19, 28, 24, 18, 12],
                    borderColor: '#0ea5e9',
                    backgroundColor: 'rgba(14, 165, 233, 0.1)',
                    borderWidth: 3,
                    fill: true,
                    tension: 0.4
                }, {
                    label: 'Yeni Planlanan',
                    data: [8, 15, 12, 19, 17, 9, 7],
                    borderColor: '#10b981',
                    backgroundColor: 'transparent',
                    borderWidth: 2,
                    borderDash: [5, 5],
                    tension: 0.4
                }]
            },
            options: {
                responsive: true,
                maintainAspectRatio: false,
                plugins: {
                    legend: { labels: { color: textColor, font: { family: 'Inter' } } }
                },
                scales: {
                    x: { grid: { color: gridColor }, ticks: { color: textColor } },
                    y: { grid: { color: gridColor }, ticks: { color: textColor } }
                }
            }
        });
    }

    // Clinic Distribution Doughnut Chart
    const ctxDist = document.getElementById('clinicDistChart');
    if (ctxDist) {
        state.distChartInstance = new Chart(ctxDist, {
            type: 'doughnut',
            data: {
                labels: ['Kardiyoloji', 'Dahiliye', 'Nöroloji', 'Ortopedi'],
                datasets: [{
                    data: [42, 35, 18, 25],
                    backgroundColor: ['#0ea5e9', '#10b981', '#8b5cf6', '#f59e0b'],
                    borderWidth: 0
                }]
            },
            options: {
                responsive: true,
                maintainAspectRatio: false,
                plugins: {
                    legend: { position: 'bottom', labels: { color: textColor } }
                }
            }
        });
    }
}

/* ==========================================================================
   Navigation & Tab Switching
   ========================================================================== */
function setupNavigation() {
    const navItems = document.querySelectorAll('.nav-item[data-tab]');
    navItems.forEach(item => {
        item.addEventListener('click', () => {
            const tabTarget = item.getAttribute('data-tab');
            switchTab(tabTarget);
        });
    });

    // Mobile sidebar toggle
    const menuBtn = document.getElementById('mobileMenuToggle');
    const sidebar = document.querySelector('.app-sidebar');
    if (menuBtn && sidebar) {
        menuBtn.addEventListener('click', () => {
            sidebar.classList.toggle('open');
        });
    }
}

function switchTab(tabId) {
    document.querySelectorAll('.nav-item[data-tab]').forEach(el => el.classList.remove('active'));
    document.querySelectorAll('.tab-pane').forEach(el => el.classList.remove('active'));

    const activeNav = document.querySelector(`.nav-item[data-tab="${tabId}"]`);
    const activePane = document.getElementById(tabId);

    if (activeNav) activeNav.classList.add('active');
    if (activePane) activePane.classList.add('active');

    // Close mobile drawer on navigation
    const sidebar = document.querySelector('.app-sidebar');
    if (sidebar) sidebar.classList.remove('open');
}

/* ==========================================================================
   Modals & Form Submissions
   ========================================================================== */
function setupModals() {
    // Open Appointment Modal
    document.querySelectorAll('[data-open-modal="appointmentModal"]').forEach(btn => {
        btn.addEventListener('click', () => openModal('appointmentModal'));
    });

    // Open Patient Modal
    document.querySelectorAll('[data-open-modal="patientModal"]').forEach(btn => {
        btn.addEventListener('click', () => openModal('patientModal'));
    });

    // Close buttons
    document.querySelectorAll('.close-modal-btn, [data-close-modal]').forEach(btn => {
        btn.addEventListener('click', (e) => {
            const modal = e.target.closest('.modal-backdrop');
            if (modal) modal.classList.remove('active');
        });
    });

    // Appointment Form Submit
    const appForm = document.getElementById('newAppointmentForm');
    if (appForm) {
        appForm.addEventListener('submit', async (e) => {
            e.preventDefault();
            await handleCreateAppointment();
        });
    }

    // Patient Form Submit
    const patientForm = document.getElementById('newPatientForm');
    if (patientForm) {
        patientForm.addEventListener('submit', async (e) => {
            e.preventDefault();
            await handleCreatePatient();
        });
    }
}

function openModal(modalId) {
    const modal = document.getElementById(modalId);
    if (modal) modal.classList.add('active');
}

function closeModal(modalId) {
    const modal = document.getElementById(modalId);
    if (modal) modal.classList.remove('active');
}

function populateModalDropdowns() {
    // Populate Patients
    const patientSelect = document.getElementById('appPatientSelect');
    if (patientSelect) {
        patientSelect.innerHTML = '<option value="">-- Hasta Seçiniz --</option>' +
            state.patients.map(p => `<option value="${p.id}">${escapeHtml(p.firstName)} ${escapeHtml(p.lastName)} (${p.nationalId})</option>`).join('');
    }

    // Populate Doctors
    const doctorSelect = document.getElementById('appDoctorSelect');
    if (doctorSelect) {
        doctorSelect.innerHTML = '<option value="">-- Doktor Seçiniz --</option>' +
            state.doctors.map(d => `<option value="${d.id}">${escapeHtml(d.fullName)} - ${escapeHtml(d.specialty)}</option>`).join('');
    }

    // Populate Insurance Types
    const insSelect = document.getElementById('patientInsuranceSelect');
    if (insSelect) {
        insSelect.innerHTML = state.insuranceTypes.map(i => `<option value="${i.id}">${escapeHtml(i.name)}</option>`).join('');
    }
}

async function handleCreateAppointment() {
    const patientId = document.getElementById('appPatientSelect').value;
    const doctorId = document.getElementById('appDoctorSelect').value;
    const dateVal = document.getElementById('appDateInput').value;
    const timeVal = document.getElementById('appTimeInput').value;
    const notes = document.getElementById('appNotesInput').value;

    if (!patientId || !doctorId || !dateVal || !timeVal) {
        alert('Lütfen hasta, doktor, tarih ve saat alanlarını doldurunuz.');
        return;
    }

    const doctor = state.doctors.find(d => d.id === doctorId);
    const clinicId = doctor ? doctor.clinicId : (state.clinics[0] ? state.clinics[0].id : null);

    const appointmentDateTime = new Date(`${dateVal}T${timeVal}:00Z`).toISOString();

    const payload = {
        patientId,
        doctorId,
        clinicId,
        appointmentDate: appointmentDateTime,
        notes: notes || 'Web Paneli üzerinden oluşturuldu'
    };

    try {
        const res = await fetch(`${API_BASE}/Appointments`, {
            method: 'POST',
            headers: getAuthHeaders(),
            body: JSON.stringify(payload)
        });

        if (res.ok) {
            closeModal('appointmentModal');
            document.getElementById('newAppointmentForm').reset();
            showToast('Randevu Oluşturuldu', 'Randevu başarıyla kaydedildi ve SignalR üzerinden yayınlandı!', 'success');
            await reloadAppointments();
        } else {
            const err = await res.json();
            alert(`Randevu oluşturulamadı: ${err.message || 'Hata'}`);
        }
    } catch (err) {
        console.error('Randevu kaydı hatası:', err);
        alert('Sunucu ile iletişim kurulurken bir hata oluştu.');
    }
}

async function handleCreatePatient() {
    const firstName = document.getElementById('patientFirstName').value.trim();
    const lastName = document.getElementById('patientLastName').value.trim();
    const nationalId = document.getElementById('patientNationalId').value.trim();
    const gender = document.getElementById('patientGender').value;
    const birthDate = document.getElementById('patientBirthDate').value;
    const phone = document.getElementById('patientPhone').value.trim();
    const email = document.getElementById('patientEmail').value.trim();
    const emergencyName = document.getElementById('patientEmergencyName').value.trim();
    const emergencyPhone = document.getElementById('patientEmergencyPhone').value.trim();
    const insuranceTypeId = parseInt(document.getElementById('patientInsuranceSelect').value) || 1;

    if (!firstName || !lastName || !nationalId || !birthDate) {
        alert('Lütfen ad, soyad, TC Kimlik ve doğum tarihi alanlarını eksiksiz doldurun.');
        return;
    }

    const payload = {
        firstName,
        lastName,
        nationalId,
        gender,
        birthDate: new Date(birthDate).toISOString(),
        phoneNumber: phone,
        email: email,
        address: 'İstanbul',
        emergencyContactName: emergencyName,
        emergencyContactPhone: emergencyPhone,
        insuranceTypeId
    };

    try {
        const res = await fetch(`${API_BASE}/Patients`, {
            method: 'POST',
            headers: getAuthHeaders(),
            body: JSON.stringify(payload)
        });

        if (res.ok) {
            closeModal('patientModal');
            document.getElementById('newPatientForm').reset();
            showToast('Hasta Eklendi', `${firstName} ${lastName} hasta kaydı başarıyla oluşturuldu.`, 'success');
            await reloadPatients();
            populateModalDropdowns();
        } else {
            const err = await res.json();
            alert(`Hasta kaydı başarısız: ${err.message || 'Lütfen alanları kontrol edin'}`);
        }
    } catch (err) {
        console.error('Hasta kayıt hatası:', err);
    }
}

/* ==========================================================================
   Appointment Status Updates
   ========================================================================== */
async function toggleAppointmentStatus(id, currentStatus) {
    const nextStatus = currentStatus === 'Scheduled' ? 'Completed' : (currentStatus === 'Completed' ? 'Cancelled' : 'Scheduled');
    await updateAppointmentStatus(id, nextStatus);
}

function changeStatusPrompt(id) {
    const statusChoice = prompt("Yeni durumu giriniz (Scheduled, Completed, Cancelled):", "Completed");
    if (statusChoice && ['Scheduled', 'Completed', 'Cancelled'].includes(statusChoice)) {
        updateAppointmentStatus(id, statusChoice);
    }
}

async function updateAppointmentStatus(id, newStatus) {
    try {
        const res = await fetch(`${API_BASE}/Appointments/${id}/status`, {
            method: 'PATCH',
            headers: getAuthHeaders(),
            body: JSON.stringify({ status: newStatus })
        });

        if (res.ok) {
            await reloadAppointments();
        } else {
            alert('Durum güncellenemedi.');
        }
    } catch (err) {
        console.error('Status update failed:', err);
    }
}

/* ==========================================================================
   Helper Quick Actions & Simulator
   ========================================================================== */
function openNewAppointmentModalForPatient(patientId) {
    openModal('appointmentModal');
    const select = document.getElementById('appPatientSelect');
    if (select) select.value = patientId;
}

function openNewAppointmentModalWithDoctor(doctorId) {
    openModal('appointmentModal');
    const select = document.getElementById('appDoctorSelect');
    if (select) select.value = doctorId;
}

// SignalR Real-Time Simulation Trigger
async function triggerRealtimeDemo() {
    if (state.patients.length === 0 || state.doctors.length === 0) {
        alert('Önce en az bir hasta ve doktor bulunmalıdır.');
        return;
    }
    const randomPatient = state.patients[Math.floor(Math.random() * state.patients.length)];
    const randomDoctor = state.doctors[Math.floor(Math.random() * state.doctors.length)];
    const randomClinic = state.clinics[0];

    const randomHour = 9 + Math.floor(Math.random() * 8);
    const randomMinute = Math.random() > 0.5 ? 30 : 0;
    const testDate = new Date();
    testDate.setHours(randomHour, randomMinute, 0, 0);

    const payload = {
        patientId: randomPatient.id,
        doctorId: randomDoctor.id,
        clinicId: randomClinic.id,
        appointmentDate: testDate.toISOString(),
        notes: '⚡ Canlı SignalR Simülasyon Randevusu'
    };

    try {
        const res = await fetch(`${API_BASE}/Appointments`, {
            method: 'POST',
            headers: getAuthHeaders(),
            body: JSON.stringify(payload)
        });
        if (res.ok) {
            showToast('Simülasyon Başarılı', 'WebSocket üzerinden anlık bildirim tüm istemcilere dağıtıldı!', 'success');
        }
    } catch (err) {
        console.error('Sim error:', err);
    }
}

/* ==========================================================================
   Search Functionality
   ========================================================================== */
function setupSearch() {
    const searchInput = document.getElementById('globalSearchInput');
    if (!searchInput) return;

    searchInput.addEventListener('input', (e) => {
        const query = e.target.value.toLowerCase().trim();
        if (!query) {
            renderPatientsTable(state.patients);
            return;
        }

        const filtered = state.patients.filter(p =>
            (p.firstName && p.firstName.toLowerCase().includes(query)) ||
            (p.lastName && p.lastName.toLowerCase().includes(query)) ||
            (p.nationalId && p.nationalId.includes(query)) ||
            (p.phoneNumber && p.phoneNumber.includes(query))
        );
        renderPatientsTable(filtered);
    });
}

/* ==========================================================================
   Utilities & Toast Notifications
   ========================================================================== */
function showToast(title, desc, type = 'info') {
    const container = document.getElementById('toastContainer');
    if (!container) return;

    const toast = document.createElement('div');
    toast.className = `toast-message toast-${type}`;
    toast.innerHTML = `
        <div class="toast-icon">
            <svg fill="none" stroke="currentColor" viewBox="0 0 24 24" width="18" height="18">
                <path stroke-linecap="round" stroke-linejoin="round" stroke-width="2" d="${type === 'success' ? 'M5 13l4 4L19 7' : 'M13 16h-1v-4h-1m1-4h.01M21 12a9 9 0 11-18 0 9 9 0 0118 0z'}" />
            </svg>
        </div>
        <div class="toast-body">
            <div class="toast-title">${escapeHtml(title)}</div>
            <div class="toast-desc">${escapeHtml(desc)}</div>
        </div>
    `;

    container.appendChild(toast);

    setTimeout(() => {
        toast.style.opacity = '0';
        toast.style.transform = 'translateY(-10px)';
        setTimeout(() => toast.remove(), 300);
    }, 4500);
}

function getStatusLabel(status) {
    switch (status) {
        case 'Scheduled': return 'Planlandı';
        case 'Completed': return 'Tamamlandı';
        case 'Cancelled': return 'İptal';
        default: return status;
    }
}

function escapeHtml(str) {
    if (!str) return '';
    return str.replace(/[&<>"']/g, (m) => ({
        '&': '&amp;',
        '<': '&lt;',
        '>': '&gt;',
        '"': '&quot;',
        "'": '&#39;'
    })[m]);
}

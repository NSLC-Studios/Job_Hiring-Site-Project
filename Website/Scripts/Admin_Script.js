const API_BASE = "https://localhost:7142";

let SelectedUserId = null;
let SelectedCompanyId = null;
let SelectedJobId = null;
let SelectedRequestId = null;

document.addEventListener("DOMContentLoaded", () => {
    InitAdmin();
});

async function InitAdmin() {
    await UserSession();
    if (UserContainer.Role !== "Admin") {
        document.getElementById("admin-error").classList.remove("hidden");
        return;
    }
    document.getElementById("admin-panel").classList.remove("hidden");
    InitTabs();
    LoadUsers();
    LoadAllCompanies();
}

function InitTabs() {
    document.querySelectorAll("#admin-tabs .nav-link").forEach(tabButton => {
        tabButton.addEventListener("click", () => {
            document.querySelectorAll("#admin-tabs .nav-link").forEach(x => x.classList.remove("active"));
            tabButton.classList.add("active");
            document.querySelectorAll(".admin-tab").forEach(tab => tab.classList.add("hidden"));
            document.getElementById(tabButton.dataset.target).classList.remove("hidden");
        });
    });
}

function ClearUserSelection() {
    document.querySelectorAll("[data-userid]").forEach(el => el.classList.remove("selected"));
}

function ClearCompanySelection() {
    document.querySelectorAll("[data-companyid]").forEach(el => el.classList.remove("selected"));
}

function ClearJobSelection() {
    document.querySelectorAll("[data-jobid]").forEach(el => el.classList.remove("selected"));
}

function ClearRequestSelection() {
    document.querySelectorAll("[data-requestid]").forEach(el => el.classList.remove("selected"));
}

function ClearUserContext() {
    document.getElementById("user-jobs-container").innerHTML = "";
    document.getElementById("user-requests-container").innerHTML = "";
    document.getElementById("user-request-details").classList.add("hidden");
}

function ClearCompanyContext() {
    document.getElementById("company-jobs-container").innerHTML = "";
    document.getElementById("company-requests-container").innerHTML = "";
    document.getElementById("company-request-details").classList.add("hidden");
}

async function LoadUsers() {
    const response = await fetch(`${API_BASE}/api/Admin/users`, { credentials: "include" });
    if (!response.ok) return;

    const users = await response.json();
    const container = document.getElementById("users-container");
    const searchInput = document.getElementById("user-search");

    function RenderUsers() {
        container.innerHTML = "";
        users
            .filter(user => user.userName.toLowerCase().includes(searchInput.value.toLowerCase()))
            .forEach(user => {
                const userItem = document.createElement("div");
                userItem.className = "list-group-item d-flex justify-content-between align-items-center";
                userItem.style.cursor = "pointer";
                userItem.dataset.userid = user.id;

                userItem.innerHTML = `
                    <div><strong>${user.userName}</strong> — ${user.role}</div>
                    <div class="btn-group">
                        <button class="btn btn-warning btn-sm" onclick="event.stopPropagation(); ResetPassword(${user.id})">Reset</button>
                        <button class="btn btn-primary btn-sm" onclick="event.stopPropagation(); Promote(${user.id})">Promote</button>
                        <button class="btn btn-secondary btn-sm" onclick="event.stopPropagation(); Demote(${user.id})">Demote</button>
                        <button class="btn btn-danger btn-sm" onclick="event.stopPropagation(); DeleteUser(${user.id})">Delete</button>
                    </div>
                `;

                userItem.addEventListener("click", () => {
                    ClearUserSelection();
                    userItem.classList.add("selected");
                    SelectedUserId = user.id;
                    SelectedCompanyId = null;
                    SelectedJobId = null;
                    SelectedRequestId = null;
                    ClearUserContext();
                    LoadUserDetails(user.id);
                    LoadUserCompanies(user.id);
                });

                container.appendChild(userItem);
            });
    }

    searchInput.addEventListener("input", RenderUsers);
    RenderUsers();
}

async function LoadUserDetails(userId) {
    const response = await fetch(`${API_BASE}/api/Admin/username?id=${userId}`, { credentials: "include" });
    if (!response.ok) return;

    const user = await response.json();
    document.getElementById("user-context").classList.remove("hidden");
    document.getElementById("user-details-content").innerHTML = `
        <p><strong>ID:</strong> ${user.id}</p>
        <p><strong>Username:</strong> ${user.userName}</p>
    `;
}

async function LoadUserCompanies(userId) {
    const response = await fetch(`${API_BASE}/api/Admin/companies?id=${userId}`, { credentials: "include" });
    if (!response.ok) return;

    const companies = await response.json();
    const container = document.getElementById("user-companies-container");
    container.innerHTML = "";

    companies.forEach(company => {
        const companyItem = document.createElement("div");
        companyItem.className = "list-group-item d-flex justify-content-between align-items-center";
        companyItem.style.cursor = "pointer";
        companyItem.dataset.companyid = company.id;

        companyItem.innerHTML = `
            <div><strong>${company.name}</strong> (Owner: ${company.ownerID})</div>
            <button class="btn btn-danger btn-sm" onclick="event.stopPropagation(); DeleteCompany(${company.id})">Delete</button>
        `;

        companyItem.addEventListener("click", () => {
            ClearCompanySelection();
            companyItem.classList.add("selected");
            SelectedCompanyId = company.id;
            SelectedJobId = null;
            SelectedRequestId = null;
            ClearUserContext();
            LoadUserCompanyJobs(company.id);
        });

        container.appendChild(companyItem);
    });
}

async function LoadUserCompanyJobs(companyId) {
    const response = await fetch(`${API_BASE}/api/Admin/jobs?id=${companyId}`, { credentials: "include" });
    if (!response.ok) return;

    const jobs = await response.json();
    const container = document.getElementById("user-jobs-container");
    container.innerHTML = "";

    jobs.forEach(job => {
        const jobItem = document.createElement("div");
        jobItem.className = "list-group-item d-flex justify-content-between align-items-center";
        jobItem.style.cursor = "pointer";
        jobItem.dataset.jobid = job.id;

        jobItem.innerHTML = `
            <div><strong>${job.description}</strong> — ${job.city}, ${job.country}</div>
            <button class="btn btn-danger btn-sm" onclick="event.stopPropagation(); DeleteJob(${job.id})">Delete</button>
        `;

        jobItem.addEventListener("click", () => {
            ClearJobSelection();
            jobItem.classList.add("selected");
            SelectedJobId = job.id;
            SelectedRequestId = null;
            document.getElementById("user-requests-container").innerHTML = "";
            LoadUserJobRequests(job.id);
        });

        container.appendChild(jobItem);
    });
}

async function LoadUserJobRequests(jobId) {
    const response = await fetch(`${API_BASE}/api/Admin/requests?id=${jobId}`, { credentials: "include" });
    if (!response.ok) return;

    const requests = await response.json();
    const container = document.getElementById("user-requests-container");
    container.innerHTML = "";

    requests.forEach(request => {
        const requestItem = document.createElement("div");
        requestItem.className = "list-group-item d-flex justify-content-between align-items-center";
        requestItem.style.cursor = "pointer";
        requestItem.dataset.requestid = request.id;

        requestItem.innerHTML = `
            <div><strong>Request #${request.id}</strong> — ${request.status}</div>
            <div class="btn-group">
                <button class="btn btn-warning btn-sm" onclick="event.stopPropagation(); PutUnderReview(${request.id})">Under Review</button>
                <button class="btn btn-danger btn-sm" onclick="event.stopPropagation(); DeleteRequest(${request.id})">Delete</button>
            </div>
        `;

        requestItem.addEventListener("click", () => {
            ClearRequestSelection();
            requestItem.classList.add("selected");
            SelectedRequestId = request.id;
            LoadUserRequestDetails(request.id);
        });

        container.appendChild(requestItem);
    });
}

async function LoadUserRequestDetails(requestId) {
    const response = await fetch(`${API_BASE}/api/Request?id=${requestId}`, { credentials: "include" });
    if (!response.ok) return;

    const request = await response.json();
    const panel = document.getElementById("user-request-details");
    panel.classList.remove("hidden");
    panel.innerHTML = `
        <p><strong>Applicant:</strong> ${request.applicant}</p>
        <p><strong>Company:</strong> ${request.companyName}</p>
        <p><strong>Job:</strong> ${request.description}</p>
        <p><strong>Status:</strong> ${request.status}</p>
        <p><strong>Comment:</strong> ${request.comment}</p>
        <button class="btn btn-warning mt-3" onclick="PutUnderReview(${request.id})">Under Review</button>
        <button class="btn btn-danger mt-3" onclick="DeleteRequest(${request.id})">Delete</button>
    `;
}

async function LoadAllCompanies() {
    const response = await fetch(`${API_BASE}/api/Admin/allcompanies`, { credentials: "include" });
    if (!response.ok) return;

    const companies = await response.json();
    const container = document.getElementById("companies-container");
    const searchInput = document.getElementById("company-search");

    function RenderCompanies() {
        container.innerHTML = "";
        companies
            .filter(company => company.name.toLowerCase().includes(searchInput.value.toLowerCase()))
            .forEach(company => {
                const companyItem = document.createElement("div");
                companyItem.className = "list-group-item d-flex justify-content-between align-items-center";
                companyItem.style.cursor = "pointer";
                companyItem.dataset.companyid = company.id;

                companyItem.innerHTML = `
                    <div><strong>${company.name}</strong> (Owner: ${company.ownerID})</div>
                    <button class="btn btn-danger btn-sm" onclick="event.stopPropagation(); DeleteCompany(${company.id})">Delete</button>
                `;

                companyItem.addEventListener("click", () => {
                    ClearCompanySelection();
                    companyItem.classList.add("selected");
                    SelectedCompanyId = company.id;
                    SelectedJobId = null;
                    SelectedRequestId = null;
                    ClearCompanyContext();
                    LoadCompanyJobs(company.id);
                });

                container.appendChild(companyItem);
            });
    }

    searchInput.addEventListener("input", RenderCompanies);
    RenderCompanies();
}

async function LoadCompanyJobs(companyId) {
    const response = await fetch(`${API_BASE}/api/Admin/jobs?id=${companyId}`, { credentials: "include" });
    if (!response.ok) return;

    const jobs = await response.json();
    const container = document.getElementById("company-jobs-container");
    document.getElementById("company-context").classList.remove("hidden");
    container.innerHTML = "";

    jobs.forEach(job => {
        const jobItem = document.createElement("div");
        jobItem.className = "list-group-item d-flex justify-content-between align-items-center";
        jobItem.style.cursor = "pointer";
        jobItem.dataset.jobid = job.id;

        jobItem.innerHTML = `
            <div><strong>${job.description}</strong> — ${job.city}, ${job.country}</div>
            <button class="btn btn-danger btn-sm" onclick="event.stopPropagation(); DeleteJob(${job.id})">Delete</button>
        `;

        jobItem.addEventListener("click", () => {
            ClearJobSelection();
            jobItem.classList.add("selected");
            SelectedJobId = job.id;
            SelectedRequestId = null;
            document.getElementById("company-requests-container").innerHTML = "";
            LoadCompanyJobRequests(job.id);
        });

        container.appendChild(jobItem);
    });
}

async function LoadCompanyJobRequests(jobId) {
    const response = await fetch(`${API_BASE}/api/Admin/requests?id=${jobId}`, { credentials: "include" });
    if (!response.ok) return;

    const requests = await response.json();
    const container = document.getElementById("company-requests-container");
    container.innerHTML = "";

    requests.forEach(request => {
        const requestItem = document.createElement("div");
        requestItem.className = "list-group-item d-flex justify-content-between align-items-center";
        requestItem.style.cursor = "pointer";
        requestItem.dataset.requestid = request.id;

        requestItem.innerHTML = `
            <div><strong>Request #${request.id}</strong> — ${request.status}</div>
            <div class="btn-group">
                <button class="btn btn-warning btn-sm" onclick="event.stopPropagation(); PutUnderReview(${request.id})">Under Review</button>
                <button class="btn btn-danger btn-sm" onclick="event.stopPropagation(); DeleteRequest(${request.id})">Delete</button>
            </div>
        `;

        requestItem.addEventListener("click", () => {
            ClearRequestSelection();
            requestItem.classList.add("selected");
            SelectedRequestId = request.id;
            LoadCompanyRequestDetails(request.id);
        });

        container.appendChild(requestItem);
    });
}

async function LoadCompanyRequestDetails(requestId) {
    const response = await fetch(`${API_BASE}/api/Request?id=${requestId}`, { credentials: "include" });
    if (!response.ok) return;

    const request = await response.json();
    const panel = document.getElementById("company-request-details");
    panel.classList.remove("hidden");
    panel.innerHTML = `
        <p><strong>Applicant:</strong> ${request.applicant}</p>
        <p><strong>Company:</strong> ${request.companyName}</p>
        <p><strong>Job:</strong> ${request.description}</p>
        <p><strong>Status:</strong> ${request.status}</p>
        <p><strong>Comment:</strong> ${request.comment}</p>
        <button class="btn btn-warning mt-3" onclick="PutUnderReview(${request.id})">Under Review</button>
        <button class="btn btn-danger mt-3" onclick="DeleteRequest(${request.id})">Delete</button>
    `;
}

async function Promote(userId) {
    await fetch(`${API_BASE}/api/Admin/user/promote?id=${userId}`, { method: "PUT", credentials: "include" });
    LoadUsers();
}

async function Demote(userId) {
    await fetch(`${API_BASE}/api/Admin/user/demote?id=${userId}`, { method: "PUT", credentials: "include" });
    LoadUsers();
}

async function ResetPassword(userId) {
    const response = await fetch(`${API_BASE}/api/Admin/reset?id=${userId}`, { method: "PUT", credentials: "include" });
    if (response.ok) alert("New password: " + await response.text());
}

async function DeleteUser(userId) {
    await fetch(`${API_BASE}/api/Admin/delete/user?id=${userId}`, { method: "DELETE", credentials: "include" });
    LoadUsers();
}

async function DeleteCompany(companyId) {
    await fetch(`${API_BASE}/api/Admin/delete/company?id=${companyId}`, { method: "DELETE", credentials: "include" });
    LoadUsers();
    LoadAllCompanies();
}

async function DeleteJob(jobId) {
    await fetch(`${API_BASE}/api/Admin/delete/job?id=${jobId}`, { method: "DELETE", credentials: "include" });
    LoadUsers();
    LoadAllCompanies();
}

async function DeleteRequest(requestId) {
    await fetch(`${API_BASE}/api/Admin/delete/request?id=${requestId}`, { method: "DELETE", credentials: "include" });
    LoadUsers();
    LoadAllCompanies();
}

async function PutUnderReview(requestId) {
    await fetch(`${API_BASE}/api/Admin/request/underreview?id=${requestId}`, { method: "PUT", credentials: "include" });
    LoadUsers();
    LoadAllCompanies();
}

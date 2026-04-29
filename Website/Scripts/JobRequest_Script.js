const API_BASE = "https://localhost:7142";

const detailsBox = document.getElementById("request-details");
const errorBox = document.getElementById("request-error");

const companyNameEl = document.getElementById("company-name");
const jobDescriptionEl = document.getElementById("job-description");
const workTimeEl = document.getElementById("job-worktime");
const languageEl = document.getElementById("job-language");
const payEl = document.getElementById("job-pay");

const applicantNameEl = document.getElementById("applicant-name");
const applicantEmailEl = document.getElementById("applicant-email");
const applicantPhoneEl = document.getElementById("applicant-phone");

const userCommentLabel = document.getElementById("user-comment-label");

const responseLabel = document.getElementById("company-response-label");
const responseInput = document.getElementById("company-response-input");
const editResponseBtn = document.getElementById("edit-response");

const statusLabel = document.getElementById("status-label");
const statusSelect = document.getElementById("status-select");
const editStatusBtn = document.getElementById("edit-status");

const viewCVBtn = document.getElementById("view-cv-bottom");
const viewProfileBtn = document.getElementById("view-profile-bottom");
const viewJobBtn = document.getElementById("view-job-bottom");

let requestID = null;
let requestData = null;

let responseEditMode = false;
let statusEditMode = false;

window.addEventListener("load", InitCompanyRequestDetails);

async function InitCompanyRequestDetails() {
    await UserSession();

    requestID = parseInt(window.location.pathname.split("/")[3]);

    await LoadRequest();

    editResponseBtn.addEventListener("click", ToggleResponseEdit);
    editStatusBtn.addEventListener("click", ToggleStatusEdit);
}

async function LoadRequest() {
    try {
        const res = await fetch(`${API_BASE}/api/Request?id=${requestID}`, {
            credentials: "include"
        });

        if (!res.ok) throw new Error();

        requestData = await res.json();
        FillRequestDetails(requestData);

        detailsBox.classList.remove("hidden");
    } catch {
        errorBox.classList.remove("hidden");
    }
}

function FillRequestDetails(r) {
    companyNameEl.innerText = r.companyName;
    jobDescriptionEl.innerText = r.description;
    workTimeEl.innerText = r.workTime;
    languageEl.innerText = r.language;
    payEl.innerText = r.pay;

    applicantNameEl.innerText = r.applicant;
    applicantEmailEl.innerText = r.email;
    applicantPhoneEl.innerText = r.phone;

    userCommentLabel.innerText = r.comment;

    responseLabel.innerText = r.response ?? "No response yet.";
    responseInput.value = r.response ?? "";

    statusLabel.innerText = r.status;
    statusSelect.value = r.status;

    viewCVBtn.href = `/CV/${r.cvid}`;
    viewProfileBtn.href = `/Profile/${r.applicantID}`;
    viewJobBtn.href = `/Job/${r.jobID}`;

    if (r.status === "UnderReview") {
        editResponseBtn.disabled = true;
        editStatusBtn.disabled = true;
    }
}

function ToggleResponseEdit() {
    if (!responseEditMode) {
        responseEditMode = true;
        responseLabel.classList.add("hidden");
        responseInput.classList.remove("hidden");
        responseInput.disabled = false;
        responseInput.focus();
        editResponseBtn.innerText = "Save";
    } else {
        SaveResponse();
    }
}

async function SaveResponse() {
    try {
        const dto = {
            id: requestID,
            response: responseInput.value.trim()
        };

        const res = await fetch(`${API_BASE}/api/Request/request/response`, {
            method: "PUT",
            credentials: "include",
            headers: { "Content-Type": "application/json" },
            body: JSON.stringify(dto)
        });

        if (!res.ok) {
            alert("This request is under review and cannot be edited.");
            return;
        }

        responseLabel.innerText = responseInput.value.trim();
        responseLabel.classList.remove("hidden");
        responseInput.classList.add("hidden");
        responseInput.disabled = true;

        editResponseBtn.innerText = "Edit";
        responseEditMode = false;

    } catch (err) {
        console.error("SaveResponse error:", err);
    }
}

function ToggleStatusEdit() {
    if (!statusEditMode) {
        statusEditMode = true;
        statusLabel.classList.add("hidden");
        statusSelect.classList.remove("hidden");
        editStatusBtn.innerText = "Save";
    } else {
        SaveStatus();
    }
}

async function SaveStatus() {
    try {
        const dto = {
            id: requestID,
            status: statusSelect.value
        };

        const res = await fetch(`${API_BASE}/api/Request/request/status`, {
            method: "PUT",
            credentials: "include",
            headers: { "Content-Type": "application/json" },
            body: JSON.stringify(dto)
        });

        if (!res.ok) {
            alert("This request is under review and cannot be edited.");
            return;
        }

        statusLabel.innerText = statusSelect.value;
        statusLabel.classList.remove("hidden");
        statusSelect.classList.add("hidden");

        editStatusBtn.innerText = "Edit";
        statusEditMode = false;

    } catch (err) {
        console.error("SaveStatus error:", err);
    }
}

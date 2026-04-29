const API_BASE = "https://localhost:7142";

const detailsBox = document.getElementById("request-details");
const errorBox = document.getElementById("request-error");

const companyNameEl = document.getElementById("company-name");
const jobDescriptionEl = document.getElementById("job-description");
const workTimeEl = document.getElementById("job-worktime");
const languageEl = document.getElementById("job-language");
const payEl = document.getElementById("job-pay");

const companyEmailEl = document.getElementById("company-email");
const companyPhoneEl = document.getElementById("company-phone");
const addressEl = document.getElementById("job-address");

const viewCompanyBottom = document.getElementById("view-company-bottom");
const viewJobBottom = document.getElementById("view-job-bottom");
const viewCVBottom = document.getElementById("view-cv-bottom");

const commentLabel = document.getElementById("user-comment-label");
const commentInput = document.getElementById("user-comment-input");
const editCommentBtn = document.getElementById("edit-comment");

const companyResponseEl = document.getElementById("company-response");
const statusEl = document.getElementById("request-status");

const deleteBtn = document.getElementById("delete-request");

let requestID = null;
let requestData = null;
let editMode = false;

window.addEventListener("load", InitRequestDetails);

async function InitRequestDetails() {
    await UserSession();

    requestID = parseInt(window.location.pathname.split("/")[2]);

    await LoadRequest();

    editCommentBtn.addEventListener("click", ToggleCommentEdit);
    deleteBtn.addEventListener("click", DeleteRequest);
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

    companyEmailEl.innerText = r.companyEmail;
    companyPhoneEl.innerText = r.companyPhone;
    addressEl.innerText = r.address;

    commentLabel.innerText = r.comment;
    commentInput.value = r.comment;

    companyResponseEl.innerText = r.response ?? "No response yet.";
    statusEl.innerText = r.status;

    viewCompanyBottom.href = `/Company/${r.companyID}`;
    viewJobBottom.href = `/Job/${r.jobID}`;
    viewCVBottom.href = `/CV/${r.cvid}`;

    if (r.status === "UnderReview") {
        editCommentBtn.disabled = true;
        deleteBtn.disabled = true;
    }
}

function ToggleCommentEdit() {
    if (!editMode) {
        editMode = true;
        commentLabel.classList.add("hidden");
        commentInput.classList.remove("hidden");
        commentInput.disabled = false;
        commentInput.focus();
        editCommentBtn.innerText = "Save";
    } else {
        UpdateComment();
    }
}

async function UpdateComment() {
    try {
        const dto = {
            id: requestID,
            comment: commentInput.value.trim()
        };

        const res = await fetch(`${API_BASE}/api/Request/request/comment`, {
            method: "PUT",
            credentials: "include",
            headers: { "Content-Type": "application/json" },
            body: JSON.stringify(dto)
        });

        if (!res.ok) {
            alert("This request is under review and cannot be edited.");
            return;
        }

        commentLabel.innerText = commentInput.value.trim();
        commentLabel.classList.remove("hidden");
        commentInput.classList.add("hidden");
        commentInput.disabled = true;

        editCommentBtn.innerText = "Edit";
        editMode = false;

    } catch (err) {
        console.error("UpdateComment error:", err);
    }
}

async function DeleteRequest() {
    if (!confirm("Are you sure you want to delete this application?")) return;

    try {
        const res = await fetch(`${API_BASE}/api/Request/request/delete?id=${requestID}`, {
            method: "DELETE",
            credentials: "include"
        });

        if (!res.ok) {
            alert("This request is under review and cannot be deleted.");
            return;
        }

        deleteBtn.innerText = "Deleted!";
        setTimeout(() => {
            window.location.href = "/Request";
        }, 1000);

    } catch (err) {
        console.error("DeleteRequest error:", err);
    }
}

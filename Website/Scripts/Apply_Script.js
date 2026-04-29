const API_BASE = "https://localhost:7142";

const stepCV = document.getElementById("apply-step-cv");
const stepComment = document.getElementById("apply-step-comment");
const stepDone = document.getElementById("apply-done");

const cvSelect = document.getElementById("cvSelect");
const noCVWarning = document.getElementById("no-cv-warning");

const saveCVChoice = document.getElementById("save-cv-choice");
const saveComment = document.getElementById("save-comment");
const skipComment = document.getElementById("skip-comment");

let selectedCVID = null;
let jobID = null;

window.addEventListener("load", InitApplyPage);

async function InitApplyPage() {
    await UserSession();

    jobID = parseInt(window.location.pathname.split("/")[2]);

    await LoadJobSummary();
    await LoadCVs();

    saveCVChoice.addEventListener("click", () => {
        if (!cvSelect.value) return;
        selectedCVID = parseInt(cvSelect.value);
        NextStep(stepCV, stepComment);
    });

    skipComment.addEventListener("click", () => SubmitApplication(null));
    saveComment.addEventListener("click", () => {
        const comment = document.getElementById("applyComment").value.trim();
        SubmitApplication(comment);
    });
}

async function LoadJobSummary() {
    try {
        const res = await fetch(`${API_BASE}/api/Job?id=${jobID}`, {
            credentials: "include"
        });

        if (!res.ok) throw new Error();

        const job = await res.json();

        document.getElementById("job-summary").innerHTML = `
            Applying for <span class="fw-bold">${job.companyName}</span><br>
            <span class="text-light">${job.description.substring(0, 80)}...</span>
        `;
    } catch (err) {
        document.getElementById("job-summary").innerText = "Failed to load job.";
    }
}

async function LoadCVs() {
    try {
        const res = await fetch(`${API_BASE}/api/CV/cvs?id=${UserContainer.UserID}`, {
            credentials: "include"
        });

        if (!res.ok) {
            cvSelect.innerHTML = `<option value="">No CVs found</option>`;
            noCVWarning.classList.remove("hidden");
            return;
        }

        const cvs = await res.json();

        if (cvs.length === 0) {
            cvSelect.innerHTML = `<option value="">No CVs found</option>`;
            noCVWarning.classList.remove("hidden");
            return;
        }

        cvSelect.innerHTML = `<option value="">Choose a CV...</option>`;

        cvs.forEach(cv => {
            const opt = document.createElement("option");
            opt.value = cv.id;
            opt.textContent = cv.summary;
            cvSelect.appendChild(opt);
        });

    } catch (err) {
        console.error("LoadCVs error:", err);
    }
}

function NextStep(current, next) {
    current.classList.add("hidden");
    next.classList.remove("hidden");
}

async function SubmitApplication(comment) {
    try {
        const dto = {
            id: jobID,
            userID: UserContainer.UserID,
            cvid: selectedCVID,
            comment: comment
        };

        const res = await fetch(`${API_BASE}/api/Request/apply`, {
            method: "POST",
            credentials: "include",
            headers: { "Content-Type": "application/json" },
            body: JSON.stringify(dto)
        });

        if (!res.ok) {
            alert("You already applied for this job!");
            return;
        }

        NextStep(stepComment, stepDone);

        setTimeout(() => {
            window.location.href = "/Request";
        }, 1500);

    } catch (err) {
        console.error("SubmitApplication error:", err);
    }
}

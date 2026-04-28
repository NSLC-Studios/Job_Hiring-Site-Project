const API_BASE = "https://localhost:7142";

const jobContainer = document.getElementById("job-container");
const template = document.getElementById("card-template");
const noJobs = document.getElementById("job-check");
const page_content_summary = document.getElementById("page-content-summary");

const creationChoice = document.getElementById("creation-choice");
const createForm = document.getElementById("create-job-form");

const stepDesc = document.getElementById("create-job-description");
const stepPay = document.getElementById("create-job-pay");
const stepWork = document.getElementById("create-job-worktime");
const stepLang = document.getElementById("create-job-language");
const stepNewArea = document.getElementById("create-new-area");
const stepArea = document.getElementById("create-job-area");
const stepDone = document.getElementById("job-done");

let createdJobID = null;
let currentCompanyID = null;

window.addEventListener("load", Init);

async function Init() {
    await UserSession();

    currentCompanyID = parseInt(window.location.pathname.split("/").pop());

    await LoadJobs();
    await LoadAreas();

    document.getElementById("start-new-job-btn").addEventListener("click", StartJobCreation);

    SetupCreationFlow();
}

async function LoadJobs() {
    try {
        const res = await fetch(`${API_BASE}/api/Job/jobs/company?id=${currentCompanyID}`, {
            credentials: "include"
        });
        if (!res.ok) throw new Error();

        const jobs = await res.json();

        jobContainer.innerHTML = "";

        if (jobs.length === 0) {
            noJobs.classList.remove("hidden");
            return;
        }

        noJobs.classList.add("hidden");

        jobs.forEach(item => {
            const clone = template.content.cloneNode(true);

            clone.querySelector(".card-owner").innerText = item.companyName;
            clone.querySelector(".card-company").innerText = item.description ?? "No description";
            clone.querySelector(".card-description").innerText =
                `${item.country}, ${item.county}, ${item.city}`;

            clone.querySelector(".card-language").innerText = item.language ?? "";
            clone.querySelector(".card-workhour").innerText = item.workTime ?? "";
            clone.querySelector(".card-pay").innerText = item.pay != null ? `€${item.pay}` : "";

            clone.querySelector(".job-checkout-btn").href = `/Job/${item.id}`;

            clone.querySelector(".job-delete-btn")
                .addEventListener("click", () => DeleteJob(item.id));

            jobContainer.appendChild(clone);
        });

    } catch (err) {
        console.error("LoadJobs error:", err);
    }
}

function StartJobCreation() {
    page_content_summary.innerText = "Create a Job!";
    creationChoice.classList.add("hidden");
    jobContainer.classList.add("hidden");
    createForm.classList.remove("hidden");

    CreateEmptyJob();
}

async function CreateEmptyJob() {
    try {
        const res = await fetch(`${API_BASE}/api/Job/create?id=${currentCompanyID}`, {
            method: "POST",
            credentials: "include"
        });
        if (!res.ok) throw new Error();

        const jobsRes = await fetch(`${API_BASE}/api/Job/jobs/company?id=${currentCompanyID}`, {
            credentials: "include"
        });
        if (!jobsRes.ok) throw new Error();

        const jobs = await jobsRes.json();
        createdJobID = jobs[jobs.length - 1].id;

    } catch (err) {
        console.error("CreateEmptyJob error:", err);
    }
}

function SetupCreationFlow() {
    document.getElementById("save-job-description").addEventListener("click", async () => {
        try {
            await UpdateField("description", { id: createdJobID, description: jobDescription.value });
            NextStep(stepDesc, stepPay);
        } catch (err) {
            console.error("Save description error:", err);
        }
    });

    document.getElementById("save-job-pay").addEventListener("click", async () => {
        try {
            await UpdateField("pay", { id: createdJobID, pay: jobPay.value });
            NextStep(stepPay, stepWork);
        } catch (err) {
            console.error("Save pay error:", err);
        }
    });

    document.getElementById("skip-job-pay").addEventListener("click", () => {
        NextStep(stepPay, stepWork);
    });

    document.getElementById("save-job-worktime").addEventListener("click", async () => {
        try {
            await UpdateField("worktime", { id: createdJobID, workTime: jobWorkTime.value });
            NextStep(stepWork, stepLang);
        } catch (err) {
            console.error("Save worktime error:", err);
        }
    });

    document.getElementById("skip-job-worktime").addEventListener("click", () => {
        NextStep(stepWork, stepLang);
    });

    document.getElementById("save-job-language").addEventListener("click", async () => {
        try {
            await UpdateField("language", { id: createdJobID, language: jobLanguage.value });
            NextStep(stepLang, stepNewArea);
        } catch (err) {
            console.error("Save language error:", err);
        }
    });

    document.getElementById("skip-job-language").addEventListener("click", () => {
        NextStep(stepLang, stepNewArea);
    });

    document.getElementById("save-new-area").addEventListener("click", async () => {
        try {
            await CreateArea();
            await LoadAreas();
            NextStep(stepNewArea, stepArea);
        } catch (err) {
            console.error("Save new area error:", err);
        }
    });

    document.getElementById("skip-new-area").addEventListener("click", async () => {
        try {
            await LoadAreas();
            NextStep(stepNewArea, stepArea);
        } catch (err) {
            console.error("Skip new area error:", err);
        }
    });

    document.getElementById("save-job-area").addEventListener("click", async () => {
        try {
            await UpdateField("area", { id: createdJobID, areaID: jobAreaSelect.value });
            NextStep(stepArea, stepDone);
            setTimeout(() => location.reload(), 1000);
        } catch (err) {
            console.error("Save job area error:", err);
        }
    });

    document.getElementById("skip-job-area").addEventListener("click", () => {
        NextStep(stepArea, stepDone);
        setTimeout(() => location.reload(), 1000);
    });
}

async function UpdateField(field, dto) {
    try {
        const res = await fetch(`${API_BASE}/api/Job/update/${field}`, {
            method: "PUT",
            credentials: "include",
            headers: { "Content-Type": "application/json" },
            body: JSON.stringify(dto)
        });
        if (!res.ok) throw new Error();
    } catch (err) {
        console.error(`UpdateField ${field} error:`, err);
    }
}

function NextStep(current, next) {
    current.querySelectorAll("input, textarea, button, select")
        .forEach(item => item.disabled = true);

    next.classList.remove("hidden");
}

async function DeleteJob(id) {
    if (!confirm("Delete this job?")) return;

    try {
        const res = await fetch(`${API_BASE}/api/Job/delete?id=${id}`, {
            method: "DELETE",
            credentials: "include"
        });
        if (!res.ok) throw new Error();

        await LoadJobs();

    } catch (err) {
        console.error("DeleteJob error:", err);
    }
}

async function LoadAreas() {
    try {
        const res = await fetch(`${API_BASE}/api/Area/user/area?id=${UserContainer.UserID}`, {
            credentials: "include"
        });
        if (!res.ok) throw new Error();

        const areas = await res.json();

        const select = document.getElementById("jobAreaSelect");
        select.innerHTML = `<option value="">Choose an area...</option>`;

        areas.forEach(item => {
            const opt = document.createElement("option");
            opt.value = item.id;
            opt.textContent = item.address;
            select.appendChild(opt);
        });

    } catch (err) {
        console.error("LoadAreas error:", err);
    }
}

async function CreateArea() {
    try {
        const dto = {
            userID: UserContainer.UserID,
            country: areaCountry.value,
            county: areaCounty.value,
            city: areaCity.value,
            postalCode: areaPostal.value,
            address: areaAddress.value
        };

        const res = await fetch(`${API_BASE}/api/Area/create`, {
            method: "POST",
            credentials: "include",
            headers: { "Content-Type": "application/json" },
            body: JSON.stringify(dto)
        });
        if (!res.ok) throw new Error();

    } catch (err) {
        console.error("CreateArea error:", err);
    }
}

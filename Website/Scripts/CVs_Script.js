const API_BASE = "https://localhost:7142";

const cvContainer = document.getElementById("cv-container");
const cvTemplate = document.getElementById("cv-card-template");
const noCVs = document.getElementById("cv-check");
const creationChoiceCV = document.getElementById("creation-choice");
const createCVForm = document.getElementById("create-cv-form");
const pageContentSummary = document.getElementById("page-content-summary");

const stepSummary = document.getElementById("cv-step-summary");
const stepNewArea = document.getElementById("cv-step-new-area");
const stepSelectArea = document.getElementById("cv-step-select-area");
const stepDone = document.getElementById("cv-done");

let createdCVID = null;

window.addEventListener("load", InitCVPage);

async function InitCVPage() {
    await UserSession();
    await LoadCVs();

    if (UserContainer.UserID == undefined || UserContainer.UserID == 0){
        noCVs.innerText = "You are not logged in!";
        creationChoiceCV.classList.add("hidden");
    } else{
        document.getElementById("start-new-cv-btn")
        .addEventListener("click", async () => {
            await CreateEmptyCV();

            createCVForm.classList.remove("hidden");
            creationChoiceCV.classList.add("hidden");
            cvContainer.classList.add("hidden");
            pageContentSummary.innerText = "Create your CV!";
        });

        SetupCreationFlow();
    }
}

async function LoadCVs() {
    try {
        const userId = UserContainer.UserID;

        const res = await fetch(`${API_BASE}/api/CV/cvs?id=${userId}`, {
            credentials: "include"
        });

        if (!res.ok) {
            console.log("No CVs found.");
            return;
        }

        const cvs = await res.json();
        if (cvs.length === 0) return;

        noCVs.classList.add("hidden");

        cvs.forEach(cv => {
            const clone = cvTemplate.content.cloneNode(true);

            clone.querySelector(".cv-summary").innerText = cv.summary;

            const location = cv.summary.includes("City:")
                ? cv.summary.split("City:")[1].trim()
                : "No Location Set";

            clone.querySelector(".cv-location").innerText = location;

            clone.querySelector(".btn-success").href = `/CV/${cv.id}`;

            const deleteBtn = clone.querySelector(".btn-danger");
            deleteBtn.addEventListener("click", () => DeleteCV(cv.id, deleteBtn));
            const editBtn = clone.querySelector(".edit-cv-btn");
            editBtn.addEventListener("click", () => StartEditCV(cv.id));

            cvContainer.appendChild(clone);
        });

    } catch (err) {
        console.error("LoadCVs error:", err);
    }
}

async function CreateEmptyCV() {
    try {
        const userId = UserContainer.UserID;

        const res = await fetch(`${API_BASE}/api/CV/create?id=${userId}`, {
            method: "POST",
            credentials: "include"
        });

        if (!res.ok) throw new Error("Failed to create CV");

        const ownedRes = await fetch(`${API_BASE}/api/CV/cvs?id=${userId}`, {
            credentials: "include"
        });

        const owned = await ownedRes.json();
        createdCVID = owned[owned.length - 1].id;

    } catch (err) {
        console.error("CreateEmptyCV error:", err);
    }
}

async function StartEditCV(id) {
    createdCVID = id;

    createCVForm.classList.remove("hidden");
    creationChoiceCV.classList.add("hidden");
    cvContainer.classList.add("hidden");
    pageContentSummary.innerText = "Edit your CV";

    stepSummary.classList.remove("hidden");
    stepNewArea.classList.add("hidden");
    stepSelectArea.classList.add("hidden");
    stepDone.classList.add("hidden");

    try {
        const res = await fetch(`${API_BASE}/api/CV?id=${id}`, {
            credentials: "include"
        });

        if (!res.ok) throw new Error("Failed to load detailed CV");

        const cv = await res.json();

        document.getElementById("cvSummary").value = cv.summary ?? "";

    } catch (err) {
        console.error("LoadDetailedCV error:", err);
    }
}


async function DeleteCV(id, deleteBtn) {
    if (!confirm("Are you sure you want to delete this CV?")) return;

    try {
        const res = await fetch(`${API_BASE}/api/CV/delete?id=${id}`, {
            method: "DELETE",
            credentials: "include"
        });

        if (!res.ok) throw new Error();

        deleteBtn.closest(".w-75").remove();
        if (cvContainer.children.length === 0) noCVs.style.display = "block";

    } catch (err) {
        console.error("DeleteCV error:", err);
    }
}

function DisableStep(step) {
    step.querySelectorAll("input, textarea, select, button")
        .forEach(x => x.disabled = true);
}

function NextStep(current, next) {
    DisableStep(current);
    next.classList.remove("hidden");
}

function SetupCreationFlow() {
    document.getElementById("save-cv-summary").addEventListener("click", async () => {
        try {
            const summary = document.getElementById("cvSummary").value.trim();
            if (summary === "" || !createdCVID) return;

            const dto = {
                id: createdCVID,
                summary: summary
            };

            await fetch(`${API_BASE}/api/CV/update/summary`, {
                method: "PUT",
                credentials: "include",
                headers: { "Content-Type": "application/json" },
                body: JSON.stringify(dto)
            });

            NextStep(stepSummary, stepNewArea);

        } catch (err) {
            console.error("UpdateSummary error:", err);
        }
    });

    document.getElementById("save-cv-new-area").addEventListener("click", async () => {
        try {
            const dto = {
                userID: UserContainer.UserID,
                country: areaCountry.value,
                county: areaCounty.value,
                city: areaCity.value,
                postalCode: areaPostal.value,
                address: areaAddress.value
            };

            await fetch(`${API_BASE}/api/Area/create`, {
                method: "POST",
                credentials: "include",
                headers: { "Content-Type": "application/json" },
                body: JSON.stringify(dto)
            });

            await LoadAreasIntoSelect();
            NextStep(stepNewArea, stepSelectArea);

        } catch (err) {
            console.error("CreateArea error:", err);
        }
    });

    document.getElementById("skip-cv-new-area").addEventListener("click", async () => {
        await LoadAreasIntoSelect();
        NextStep(stepNewArea, stepSelectArea);
    });

    document.getElementById("save-cv-area").addEventListener("click", async () => {
        try {
            const areaID = document.getElementById("cvAreaSelect").value;

            if (!areaID || createdCVID == null) {
                NextStep(stepSelectArea, stepDone);
                setTimeout(() => location.reload(), 1000);
                return;
            }

            const dto = {
                id: createdCVID,
                areaID: areaID
            };

            await fetch(`${API_BASE}/api/CV/update/area`, {
                method: "PUT",
                credentials: "include",
                headers: { "Content-Type": "application/json" },
                body: JSON.stringify(dto)
            });

            NextStep(stepSelectArea, stepDone);
            setTimeout(() => location.reload(), 1000);

        } catch (err) {
            console.error("UpdateArea error:", err);
        }
    });

    document.getElementById("skip-cv-area").addEventListener("click", () => {
        NextStep(stepSelectArea, stepDone);
        setTimeout(() => location.reload(), 1000);
    });
}

async function LoadAreasIntoSelect() {
    try {
        const res = await fetch(`${API_BASE}/api/Area/user/area?id=${UserContainer.UserID}`, {
            credentials: "include"
        });

        if (!res.ok) throw new Error();

        const areas = await res.json();

        const select = document.getElementById("cvAreaSelect");
        select.innerHTML = `<option value="">Choose an area...</option>`;

        areas.forEach(a => {
            const opt = document.createElement("option");
            opt.value = a.id;
            opt.textContent = a.address;
            select.appendChild(opt);
        });

    } catch (err) {
        console.error("LoadAreas error:", err);
    }
}

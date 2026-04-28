const API_BASE = "https://localhost:7142";

const cvContainer = document.getElementById("cv-container");
const cvTemplate = document.getElementById("cv-card-template");
const noCVs = document.getElementById("cv-check");
const creationChoiceCV = document.getElementById("creation-choice");
const createCVForm = document.getElementById("create-cv-form");

window.addEventListener("load", InitCVPage);

async function InitCVPage() {
    await UserSession();
    await LoadCVs();

    document.getElementById("start-new-cv-btn")
        .addEventListener("click", async () => {
            createCVForm.classList.remove("hidden");
            creationChoiceCV.classList.add("hidden");
            cvContainer.classList.add("hidden");

            await CreateCV();
        });
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
            clone.querySelector(".cv-location").innerText = cv.summary.includes("City:")
                ? cv.summary.split("City:")[1].trim()
                : "No Location Set";

            const openBtn = clone.querySelector(".btn-success");
            openBtn.href = `/CV/${cv.id}`;

            const deleteBtn = clone.querySelector(".btn-danger");
            deleteBtn.addEventListener("click", () => DeleteCV(cv.id, deleteBtn));

            cvContainer.appendChild(clone);
        });

    } catch (err) {
        console.error("LoadCVs error:", err);
    }
}

async function CreateCV() {
    try {
        const userId = UserContainer.UserID;

        const res = await fetch(`${API_BASE}/api/CV/create?id=${userId}`, {
            method: "POST",
            credentials: "include"
        });

        if (!res.ok) throw new Error("Failed to create CV");

        setTimeout(() => location.reload(), 800);

    } catch (err) {
        console.error("CreateCV error:", err);
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

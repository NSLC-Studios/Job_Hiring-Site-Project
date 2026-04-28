const API_BASE = "https://localhost:7142";

const container = document.getElementById("company-container");
const template = document.getElementById("card-template");
const noCompanies = document.getElementById("company-check");

const form = document.getElementById("create-company-form");

const stepName = document.getElementById("create-company-name");
const stepContacts = document.getElementById("update-company-contacts");
const stepDescription = document.getElementById("update-company-description");
const stepNewArea = document.getElementById("create-new-area");
const stepSelectArea = document.getElementById("update-company-area");
const stepDone = document.getElementById("company-done");

let createdCompanyID = null;

window.addEventListener("load", Init);

async function Init() {
    await UserSession();
    await LoadCompanies();

    document.getElementById("start-new-company-btn")
        .addEventListener("click", () => form.classList.remove("hidden"));

    SetupCreationFlow();
}

async function LoadCompanies() {
    try {
        const userId = UserContainer.UserID;

        const res = await fetch(`${API_BASE}/api/Company/companies/user?id=${userId}`, {
            credentials: "include"
        });

        if (!res.ok) throw new Error("Failed to load companies");

        const companies = await res.json();

        if (companies.length === 0) {
            noCompanies.style.display = "block";
            return;
        }

        noCompanies.style.display = "none";

        companies.forEach(company => {
            const clone = template.content.cloneNode(true);

            clone.querySelector(".card-owner").innerText = company.companyName;

            const parts = company.description.split("\n");
            clone.querySelector(".card-company").innerText = parts[0];
            clone.querySelector(".card-description").innerText = parts[1] ?? "";

            clone.querySelector(".btn-success").href = `/company/${company.id}`;

            const deleteBtn = clone.querySelector(".btn-danger");
            deleteBtn.addEventListener("click", () => DeleteCompany(company.id, deleteBtn));

            container.appendChild(clone);
        });

    } catch (err) {
        console.error("LoadCompanies error:", err);
    }
}

async function DeleteCompany(id, deleteBtn) {
    if (!confirm("Are you sure you want to delete this company?")) return;

    try {
        const res = await fetch(`${API_BASE}/api/Company/delete?id=${id}`, {
            method: "DELETE",
            credentials: "include"
        });

        if (!res.ok) throw new Error();

        deleteBtn.closest(".w-75").remove();
        if (container.children.length === 0) noCompanies.style.display = "block";

    } catch (err) {
        console.error("DeleteCompany error:", err);
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
    document.getElementById("save-company-name").addEventListener("click", async () => {
        try {
            const name = document.getElementById("companyName").value.trim();
            if (name === "") return;

            const dto = {
                companyName: name,
                ownerID: UserContainer.UserID
            };

            const res = await fetch(`${API_BASE}/api/Company/create`, {
                method: "POST",
                credentials: "include",
                headers: { "Content-Type": "application/json" },
                body: JSON.stringify(dto)
            });

            if (!res.ok) {
                console.log("Company name already taken.");
                return;
            }

            const ownedRes = await fetch(`${API_BASE}/api/Company/companies/user?id=${UserContainer.UserID}`, {
                credentials: "include"
            });

            const owned = await ownedRes.json();
            createdCompanyID = owned[owned.length - 1].id;

            NextStep(stepName, stepContacts);

        } catch (err) {
            console.error("CreateCompany error:", err);
        }
    });

    document.getElementById("save-contacts").addEventListener("click", async () => {
        try {
            const dto = {
                id: createdCompanyID,
                email: document.getElementById("companyEmail").value,
                phone: document.getElementById("companyPhone").value
            };

            await fetch(`${API_BASE}/api/Company/update/contacts`, {
                method: "PUT",
                credentials: "include",
                headers: { "Content-Type": "application/json" },
                body: JSON.stringify(dto)
            });

            NextStep(stepContacts, stepDescription);

        } catch (err) {
            console.error("UpdateContacts error:", err);
        }
    });

    document.getElementById("skip-contacts").addEventListener("click", () =>
        NextStep(stepContacts, stepDescription)
    );

    document.getElementById("save-description").addEventListener("click", async () => {
        try {
            const dto = {
                id: createdCompanyID,
                description: document.getElementById("companyDescription").value
            };

            await fetch(`${API_BASE}/api/Company/update/description`, {
                method: "PUT",
                credentials: "include",
                headers: { "Content-Type": "application/json" },
                body: JSON.stringify(dto)
            });

            NextStep(stepDescription, stepNewArea);

        } catch (err) {
            console.error("UpdateDescription error:", err);
        }
    });

    document.getElementById("skip-description").addEventListener("click", () =>
        NextStep(stepDescription, stepNewArea)
    );

    document.getElementById("save-new-area").addEventListener("click", async () => {
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

    document.getElementById("skip-new-area").addEventListener("click", async () => {
        await LoadAreasIntoSelect();
        NextStep(stepNewArea, stepSelectArea);
    });

    document.getElementById("save-area").addEventListener("click", async () => {
        try {
            const dto = {
                id: createdCompanyID,
                areaID: document.getElementById("areaSelect").value
            };

            await fetch(`${API_BASE}/api/Company/update/area`, {
                method: "PUT",
                credentials: "include",
                headers: { "Content-Type": "application/json" },
                body: JSON.stringify(dto)
            });

            NextStep(stepSelectArea, stepDone);
            setTimeout(() => location.reload(), 5000);

        } catch (err) {
            console.error("UpdateArea error:", err);
        }
    });

    document.getElementById("skip-area").addEventListener("click", () => {
        NextStep(stepSelectArea, stepDone);
        setTimeout(() => location.reload(), 5000);
    });
}

async function LoadAreasIntoSelect() {
    try {
        const res = await fetch(`${API_BASE}/api/Area/user/area?id=${UserContainer.UserID}`, {
            credentials: "include"
        });

        if (!res.ok) throw new Error();

        const areas = await res.json();

        const select = document.getElementById("areaSelect");
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

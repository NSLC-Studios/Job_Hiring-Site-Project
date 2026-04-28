const container = document.getElementById("company-container");
const template = document.getElementById("card-template");
const noCompanies = document.getElementById("company-check");

window.addEventListener("load", Init)

async function Init() {
    await UserSession();

    await LoadCompanies();
}

async function LoadCompanies() {
    const userId = UserContainer.UserID;

    const res = await fetch(`/companies/user?id=${userId}`);
    const companies = await res.json();

    if (companies.length === 0) {
        noCompanies.style.display = "block";
        return;
    }

    noCompanies.style.display = "none";

    companies.forEach(company => {
        const clone = template.content.cloneNode(true);

        // Fill template fields
        clone.querySelector(".card-owner").innerText = company.companyName;

        const parts = company.description.split("\n");
        clone.querySelector(".card-company").innerText = parts[0];
        clone.querySelector(".card-description").innerText = parts[1] ?? "";

        // Assign company link
        clone.querySelector(".btn-success").href = `/company/${company.id}`;

        // Delete button
        const deleteBtn = clone.querySelector(".btn-danger");
        deleteBtn.addEventListener("click", () => DeleteCompany(company.id, deleteBtn));

        container.appendChild(clone);
    });
}

async function DeleteCompany(id, deleteBtn) {
    if (!confirm("Are you sure you want to delete this company?")) return;

    const res = await fetch(`/delete?id=${id}`, {
        method: "DELETE"
    });

    if (res.ok) {
        deleteBtn.closest(".w-75").remove();

        if (container.children.length === 0) {
            noCompanies.style.display = "block";
        }
    } else {
        alert("Failed to delete company.");
    }
}

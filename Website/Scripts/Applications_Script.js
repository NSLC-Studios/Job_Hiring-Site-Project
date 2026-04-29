const API_BASE = "https://localhost:7142";

const requestContainer = document.getElementById("request-container");
const requestTemplate = document.getElementById("request-card-template");
const noRequests = document.getElementById("request-check");

let jobID = null;

window.addEventListener("load", InitRequests);

async function InitRequests() {
    await UserSession();

    jobID = parseInt(window.location.pathname.split("/")[2]);

    await LoadRequests();
}

async function LoadRequests() {
    try {
        const res = await fetch(`${API_BASE}/api/Request/requests/job?id=${jobID}`, {
            credentials: "include"
        });

        if (!res.ok) throw new Error();

        const requests = await res.json();

        if (requests.length === 0) return;

        noRequests.classList.add("hidden");

        requests.forEach(req => {
            const clone = requestTemplate.content.cloneNode(true);

            clone.querySelector(".card-owner").innerText = req.applicant;
            clone.querySelector(".card-company").innerText = `Status: ${req.status}`;
            clone.querySelector(".card-description").innerText = req.comment;

            clone.querySelector(".btn-success").href = `/Request/job/${req.id}`;

            const deleteBtn = clone.querySelector(".btn-danger");
            deleteBtn.addEventListener("click", () => DeleteRequest(req.id, deleteBtn));

            requestContainer.appendChild(clone);
        });

    } catch (err) {
        console.error("LoadRequests error:", err);
    }
}

async function DeleteRequest(id, deleteBtn) {
    if (!confirm("Delete this application?")) return;

    try {
        const res = await fetch(`${API_BASE}/api/Request/request/delete?id=${id}`, {
            method: "DELETE",
            credentials: "include"
        });

        if (!res.ok) {
            alert("This request is under review and cannot be deleted.");
            return;
        }

        deleteBtn.closest(".w-75").remove();

        if (requestContainer.children.length === 0)
            noRequests.style.display = "block";

    } catch (err) {
        console.error("DeleteRequest error:", err);
    }
}

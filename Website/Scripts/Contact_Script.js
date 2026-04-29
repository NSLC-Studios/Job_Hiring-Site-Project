const API_BASE = "https://localhost:7142";

window.addEventListener("load", LoadAdmins);

async function LoadAdmins() {
    const container = document.getElementById("admins-container");
    const template = document.getElementById("admin-card-template");

    container.innerHTML = "";

    try {
        const res = await fetch(`${API_BASE}/api/User/admins`, {
            credentials: "include"
        });

        if (!res.ok) {
            console.error("Failed to load admins");
            return;
        }

        const admins = await res.json();

        admins.forEach(item => {
            const clone = template.content.cloneNode(true);

            clone.querySelector(".admin-name").innerText = item.name;
            clone.querySelector(".admin-username").innerText = `@${item.userName}`;
            clone.querySelector(".admin-email").innerText = item.email;
            clone.querySelector(".admin-profile-btn").href = `/Profile/${item.id}`;

            container.appendChild(clone);
        });
    } catch (err) {
        console.error("Error loading admins:", err);
    }
}

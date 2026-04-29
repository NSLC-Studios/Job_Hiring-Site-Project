const API_BASE = "https://localhost:7142";

window.addEventListener("load", LoadCV);

async function LoadCV() {
    const id = window.location.pathname.split("/")[2];

    try {
        const res = await fetch(`${API_BASE}/api/CV?id=${id}`, {
            credentials: "include"
        });

        if (!res.ok) throw new Error("Failed to load CV");

        const cv = await res.json();

        document.getElementById("cv-name").innerText = `${cv.firstName} ${cv.lastName}`;
        document.getElementById("cv-role").innerText = cv.role;
        document.getElementById("cv-summary").innerText = cv.summary ?? "No summary provided.";

        document.getElementById("cv-email").innerText = cv.email ?? "N/A";
        document.getElementById("cv-phone").innerText = cv.phone ?? "N/A";

        document.getElementById("cv-location").innerText =
            `${cv.country ?? ""}, ${cv.county ?? ""}, ${cv.city ?? ""}, ${cv.address ?? ""}`
            .replace(/, ,/g, ",")
            .replace(/^,|,$/g, "");

        document.getElementById("cv-companies").innerText = cv.companies ?? "None";

    } catch (err) {
        console.error("LoadCV error:", err);
        document.getElementById("cv-sheet").innerHTML = "<h2>Failed to load CV.</h2>";
    }
}

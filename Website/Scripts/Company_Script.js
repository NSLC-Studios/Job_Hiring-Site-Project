let CompanyId = 0;

const name_label = document.getElementById("name-label");
const username_lab = document.getElementById("username-label");
const email_label = document.getElementById("email-label");
const phone_label = document.getElementById("phone-label");

const name_group = document.getElementById("name-group");
const email_group = document.getElementById("email-group");
const phone_group = document.getElementById("phone-group");
const address_group = document.getElementById("address-group");

const edit_profile_btn = document.getElementById("edit-profile-btn");
const create_job_btn = document.getElementById("create-job");

const edit_about_btn = document.getElementById("edit-desc-btn");
const about_group = document.getElementById("edit-desc-group");
const save_about_btn = document.getElementById("save-desc-btn");
const close_about_btn = document.getElementById("close-desc-btn");
const save_address_btn = document.getElementById("address-btn");

const about_area = document.getElementById("area-description");
const about_text = document.getElementById("text-description");

const company_name = document.getElementById("company-name");
const email = document.getElementById("email");
const phone = document.getElementById("phone");
const company_address = document.getElementById("company-address");
const company_location = document.getElementById("company-location");

const job_container = document.getElementById("job-container");
const job_check = document.getElementById("job-check");
const job_template = document.getElementById("card-template");

let edit_mode = false;
let about_edit_mode = false;

window.addEventListener("load", Init);

edit_profile_btn.addEventListener("click", ToggleEditMode);

edit_about_btn.addEventListener("click", EnterAboutEdit);
save_about_btn.addEventListener("click", SaveAbout);
close_about_btn.addEventListener("click", CloseAboutEdit);
save_address_btn.addEventListener("click", UpdateArea);

document.getElementById("company-btn").addEventListener("click", UpdateCompanyName);
document.getElementById("cont-btn").addEventListener("click", UpdateCompanyContacts);

async function Init() {
    await UserSession();

    const temp = window.location.pathname.split("/");
    CompanyId = parseInt(temp[2]);

    await GetCompany();
    await GetCompanyJobs();
}

async function GetCompany() {
    try {
        const response = await fetch(`https://localhost:7142/api/Company/company?id=${CompanyId}`);

        if (!response.ok) {
            console.log("GET COMPANY - BAD RESPONSE");
            return null;
        }

        const data = await response.json();

        name_label.innerText = data.name;

        data.owner == " "
            ? username_lab.innerText = "User's name not set"
            : username_lab.innerText = `${data.owner}`;
        username_lab.href = `/Profile/${data.ownerID}`;

        email_label.innerText = data.email ?? "";
        phone_label.innerText = data.phone ?? "";

        company_name.value = data.name;
        email.value = data.email ?? "";
        phone.value = data.phone ?? "";
        data.country == null 
            ? company_address.innerText =
        "Company has no address."
            : company_address.innerText =
            `${data.country}, ${data.county}, ${data.city}, ${data.postal}, ${data.address}`;
        about_text.innerText = data.description && data.description.length > 0
            ? data.description
            : "No description set yet.";

        about_area.value = data.description ?? "";

        const owner = UserContainer.UserID == data.ownerID;
        edit_profile_btn.classList.toggle("hidden", !owner);
        edit_about_btn.classList.toggle("hidden", !owner);

        create_job_btn.href = `/CreateJob/${CompanyId}`;
        create_job_btn.classList.toggle("hidden", !owner);

    } catch (e) {
        console.log("GET COMPANY");
        console.log(e);
        return null;
    }
}

async function GetArea() {
    try {
        const response = await fetch(`https://localhost:7142/api/area/user/area?id=${UserContainer.UserID}`, {
            credentials: "include"
        });

        if (!response.ok) {
            console.log("GET AREA - BAD RESPONSE");
            return null;
        }

        const data = await response.json();

        company_location.innerHTML = "<option value=''>Choose an area...</option>";
        data.forEach(item => {
            const area = document.createElement("option");
            area.value = item.id;
            area.innerText = item.address;

            const tempAddress = company_address.innerText.split(",").pop().trim().replace("...", "").toLowerCase();
            const compareAddress = item.address.split(",").pop().trim().replace("...", "").toLowerCase();

            if (tempAddress.includes(compareAddress) || compareAddress.includes(tempAddress)) area.selected = true;

            company_location.appendChild(area);
        });

    } catch (e) {
        console.log("GET AREA");
        console.log(e);
        return null;
    }
}

async function GetCompanyJobs() {
    try {
        const response = await fetch(`https://localhost:7142/api/Job/jobs/company?id=${CompanyId}`);

        if (!response.ok) {
            console.log("GET COMPANY JOBS - BAD RESPONSE");
            return;
        }

        RenderJobCards(await response.json());
    } catch (e) {
        console.log("GET COMPANY JOBS");
        console.log(e);
    }
}

function RenderJobCards(data) {
    job_container.innerHTML = "";

    if (!data || data.length === 0) {
        job_check.innerText = "Company has no jobs!";
        return;
    }

    job_check.innerText = "";

    data.forEach(item => {
        const template = job_template.content.cloneNode(true);

        template.querySelector(".card-company").innerText = item.companyName;
        template.querySelector(".card-language").innerText = item.language;
        template.querySelector(".card-description").innerText = item.description;
        item.country == null 
            ? template.querySelector(".card-address").innerText =
        "Job has no address."
            : template.querySelector(".card-address").innerText =
            `${item.country}, ${item.county}, ${item.city}`;
        template.querySelector(".card-workhour").innerText = item.workTime;
        template.querySelector(".card-pay").innerText = `${item.pay}€`;
        template.querySelector(".card-button").href = `/Job/${item.id}`;

        job_container.appendChild(template);
    });
}

async function ToggleEditMode() {
    edit_mode = !edit_mode;

    if (edit_mode) await GetArea();

    name_group.classList.toggle("hidden");
    email_group.classList.toggle("hidden");
    phone_group.classList.toggle("hidden");
    address_group.classList.toggle("hidden");

    name_label.classList.toggle("hidden");
    email_label.classList.toggle("hidden");
    phone_label.classList.toggle("hidden");
    company_address.classList.toggle("hidden");

    edit_profile_btn.innerText = edit_mode
        ? "Finish Editing Profile"
        : "Edit Profile";
}

function EnterAboutEdit() {
    about_edit_mode = true;

    edit_about_btn.classList.add("hidden");
    about_group.classList.remove("hidden");

    about_text.classList.add("hidden");
    about_area.classList.remove("hidden");

    about_area.value = about_text.innerText;
}

function CloseAboutEdit() {
    about_edit_mode = false;

    edit_about_btn.classList.remove("hidden");
    about_group.classList.add("hidden");

    about_text.classList.remove("hidden");
    about_area.classList.add("hidden");
}

async function SaveAbout() {
    try {
        const response = await fetch(`https://localhost:7142/api/Company/update/description`, {
            method: "PUT",
            headers: { "Content-Type": "application/json" },
            credentials: "include",
            body: JSON.stringify({
                id: CompanyId,
                description: about_area.value
            })
        });

        if (response.ok) {
            about_text.innerText = about_area.value;
        } else {
            console.log("SAVE COMPANY DESCRIPTION - BAD RESPONSE");
        }
    } catch (e) {
        console.log("SAVE COMPANY DESCRIPTION");
        console.log(e);
    }

    CloseAboutEdit();
}

async function UpdateArea() {
    try {
        if (company_location.value == "") return;

        const response = await fetch(`https://localhost:7142/api/Company/update/area`, {
            method: "PUT",
            headers: { "Content-Type": "application/json" },
            credentials: "include",
            body: JSON.stringify({
                id: CompanyId,
                AreaID: company_location.value
            })
        });

        if (response.ok) {
            company_address.innerText = company_location.options[company_location.selectedIndex].text;
            company_address.innerText = company_location.options[company_location.selectedIndex].text;
        } else {
            console.log("UPDATE AREA - BAD RESPONSE");
        }
    } catch (e) {
        console.log("UPDATE AREA");
        console.log(e);
    }
}

async function UpdateCompanyName() {
    try {
        const response = await fetch(`https://localhost:7142/api/Company/update/name`, {
            method: "PUT",
            headers: { "Content-Type": "application/json" },
            credentials: "include",
            body: JSON.stringify({
                id: CompanyId,
                name: company_name.value
            })
        });

        if (response.ok) {
            name_label.innerText = company_name.value;
        } else {
            console.log("UPDATE COMPANY NAME - BAD RESPONSE");
        }
    } catch (e) {
        console.log("UPDATE COMPANY NAME");
        console.log(e);
    }
}

async function UpdateCompanyContacts() {
    try {
        const response = await fetch(`https://localhost:7142/api/Company/update/contacts`, {
            method: "PUT",
            headers: { "Content-Type": "application/json" },
            credentials: "include",
            body: JSON.stringify({
                id: CompanyId,
                email: email.value,
                phone: phone.value
            })
        });

        if (response.ok) {
            email_label.innerText = email.value;
            phone_label.innerText = phone.value;
        } else {
            console.log("UPDATE COMPANY CONTACTS - BAD RESPONSE");
        }
    } catch (e) {
        console.log("UPDATE COMPANY CONTACTS");
        console.log(e);
    }
}

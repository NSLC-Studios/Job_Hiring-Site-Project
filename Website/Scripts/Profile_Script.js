const card_template = document.getElementById("card-template");
const container = document.getElementById("company-container");
const check = document.getElementById("company-check");

// LABELS
const name_label = document.getElementById("name-label");
const username_label = document.getElementById("username-label");
const email_label = document.getElementById("email-label");
const phone_label = document.getElementById("phone-label");

// GROUPS
const name_group = document.getElementById("name-group");
const username_group = document.getElementById("username-group");
const email_group = document.getElementById("email-group");
const phone_group = document.getElementById("phone-group");

// PROFILE BUTTON
const edit_profile_btn = document.getElementById("edit-profile-btn");

// DESCRIPTION UI
const edit_desc_btn = document.getElementById("edit-desc-btn");
const desc_group = document.getElementById("edit-desc-group");
const save_desc_btn = document.getElementById("save-desc-btn");
const close_desc_btn = document.getElementById("close-desc-btn");
const area_description = document.getElementById("area-description");
const text_description = document.getElementById("text-description");

// INPUTS
const firstname = document.getElementById("firstname");
const lastname = document.getElementById("lastname");
const username = document.getElementById("username");
const email = document.getElementById("email");
const phone = document.getElementById("phone");

// STATE
let edit_mode = false;
let desc_edit_mode = false;

// IMPORTANT: profile being viewed
let profileUserId = 0;

// EVENTS
window.addEventListener("load", Init);

edit_profile_btn.addEventListener("click", ToggleEdit);

edit_desc_btn.addEventListener("click", EnterDescriptionEdit);
save_desc_btn.addEventListener("click", SaveDescription);
close_desc_btn.addEventListener("click", CloseDescriptionEdit);

// PROFILE UPDATE BUTTONS (FIXED — THIS WAS MISSING BEFORE)
document.getElementById("name-btn").addEventListener("click", UpdateName);
document.getElementById("username-btn").addEventListener("click", UpdateUsername);
document.getElementById("email-btn").addEventListener("click", UpdateContact);
document.getElementById("phone-btn").addEventListener("click", UpdateContact);

// INIT
async function Init() {
    await UserSession();

    profileUserId = GetProfileUserIdFromUrl();

    await GetUser();
    await GetCompanies();
}

// GET PROFILE ID FROM URL
function GetProfileUserIdFromUrl() {
    const parts = window.location.pathname.split("/");
    return parseInt(parts[parts.length - 1]);
}

// =====================
// USER
// =====================
async function GetUser() {
    try {
        const response = await fetch(
            `https://localhost:7142/api/User?id=${profileUserId}`
        );

        if (!response.ok) return;

        const data = await response.json();

        name_label.innerText = `${data.firstName} ${data.lastName}`;
        username_label.innerText = `@${data.userName}`;
        email_label.innerText = data.email;
        phone_label.innerText = data.phone;

        document.getElementById("profile-role").innerText = data.role;

        firstname.value = data.firstName;
        lastname.value = data.lastName;
        username.value = data.userName;
        email.value = data.email;
        phone.value = data.phone;

        text_description.innerText =
            data.about && data.about.length > 0
                ? data.about
                : "No description set yet.";

        area_description.value = data.about ?? "";

        const isOwner = UserContainer.UserID === profileUserId;

        edit_profile_btn.classList.toggle("hidden", !isOwner);
        edit_desc_btn.classList.toggle("hidden", !isOwner);
    } catch (e) {
        console.log("GET USER", e);
    }
}

// =====================
// COMPANIES
// =====================
async function GetCompanies() {
    try {
        const response = await fetch(
            `https://localhost:7142/api/Company/companies/user?id=${profileUserId}`
        );

        if (!response.ok) return;

        const data = await response.json();

        container.innerHTML = "";

        const summary = document.getElementById("company-summary");

        if (!data || data.length === 0) {
            summary.classList.add("hidden");
            summary.innerText = "";
            check.innerText = "";
            return;
        }

        summary.classList.remove("hidden");
        summary.innerText = "Proud owner of:";

        check.innerText = "";

        data.forEach((element) => {
            const template = card_template.content.cloneNode(true);

            template.querySelector(".card-owner").textContent =
                element.ownerName;
            template.querySelector(".card-company").textContent =
                element.companyName;
            template.querySelector(".card-description").textContent =
                element.description;
            template.querySelector(".card-button").href =
                `/Company/${element.id}`;

            container.appendChild(template);
        });
    } catch (e) {
        console.log("GET COMPANIES", e);
    }
}

// =====================
// PROFILE EDIT TOGGLE
// =====================
function ToggleEdit() {
    edit_mode = !edit_mode;

    name_group.classList.toggle("hidden");
    username_group.classList.toggle("hidden");
    email_group.classList.toggle("hidden");
    phone_group.classList.toggle("hidden");

    name_label.classList.toggle("hidden");
    username_label.classList.toggle("hidden");
    email_label.classList.toggle("hidden");
    phone_label.classList.toggle("hidden");

    edit_profile_btn.innerText = edit_mode
        ? "Finish Editing Profile"
        : "Edit Profile";
}

// =====================
// DESCRIPTION
// =====================
function EnterDescriptionEdit() {
    desc_edit_mode = true;

    edit_desc_btn.classList.add("hidden");
    desc_group.classList.remove("hidden");

    text_description.classList.add("hidden");
    area_description.classList.remove("hidden");

    area_description.value = text_description.innerText;
}

function CloseDescriptionEdit() {
    desc_edit_mode = false;

    edit_desc_btn.classList.remove("hidden");
    desc_group.classList.add("hidden");

    text_description.classList.remove("hidden");
    area_description.classList.add("hidden");
}

async function SaveDescription() {
    try {
        const response = await fetch(
            `https://localhost:7142/api/User/update/about`,
            {
                method: "PUT",
                headers: { "Content-Type": "application/json" },
                credentials: "include",
                body: JSON.stringify({
                    id: profileUserId,
                    about: area_description.value,
                }),
            }
        );

        if (response.ok) {
            text_description.innerText = area_description.value;
        }
    } catch (e) {
        console.log("SAVE DESCRIPTION", e);
    }

    CloseDescriptionEdit();
}

// =====================
// UPDATES (FIXED)
// =====================
async function UpdateName() {
    try {
        const response = await fetch(
            `https://localhost:7142/api/User/update/name`,
            {
                method: "PUT",
                headers: { "Content-Type": "application/json" },
                credentials: "include",
                body: JSON.stringify({
                    id: profileUserId,
                    firstName: firstname.value,
                    lastName: lastname.value,
                }),
            }
        );

        if (response.ok) {
            name_label.innerText = `${firstname.value} ${lastname.value}`;
        }
    } catch (e) {
        console.log("UPDATE NAME", e);
    }
}

async function UpdateUsername() {
    try {
        const response = await fetch(
            `https://localhost:7142/api/User/update/username`,
            {
                method: "PUT",
                headers: { "Content-Type": "application/json" },
                credentials: "include",
                body: JSON.stringify({
                    id: profileUserId,
                    userName: username.value,
                }),
            }
        );

        if (response.ok) {
            username_label.innerText = `@${username.value}`;
        }
    } catch (e) {
        console.log("UPDATE USERNAME", e);
    }
}

async function UpdateContact() {
    try {
        const response = await fetch(
            `https://localhost:7142/api/User/update/contact`,
            {
                method: "PUT",
                headers: { "Content-Type": "application/json" },
                credentials: "include",
                body: JSON.stringify({
                    id: profileUserId,
                    email: email.value,
                    phone: phone.value,
                }),
            }
        );

        if (response.ok) {
            email_label.innerText = email.value;
            phone_label.innerText = phone.value;
        }
    } catch (e) {
        console.log("UPDATE CONTACT", e);
    }
}
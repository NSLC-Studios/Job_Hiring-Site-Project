const card_template = document.getElementById("card-template");
const container = document.getElementById("company-container");
const check = document.getElementById("company-check");

const name_label = document.getElementById("name-label");
const username_label = document.getElementById("username-label");
const email_label = document.getElementById("email-label");
const phone_label = document.getElementById("phone-label");

const name_group = document.getElementById("name-group");
const username_group = document.getElementById("username-group");
const email_group = document.getElementById("email-group");
const phone_group = document.getElementById("phone-group");

const edit_profile_btn = document.getElementById("edit-profile-btn");

const edit_about_btn = document.getElementById("edit-desc-btn");
const about_group = document.getElementById("edit-desc-group");
const save_about_btn = document.getElementById("save-desc-btn");
const close_about_btn = document.getElementById("close-desc-btn");

const about_area = document.getElementById("area-description");
const about_text = document.getElementById("text-description");

const firstname = document.getElementById("firstname");
const lastname = document.getElementById("lastname");
const username = document.getElementById("username");
const email = document.getElementById("email");
const phone = document.getElementById("phone");

const summary = document.getElementById("company-summary");

const btn_update_name = document.getElementById("name-btn");
const btn_update_username = document.getElementById("username-btn");
const btn_update_contact = document.getElementById("cont-btn");

let edit_mode = false;
let about_edit_mode = false;
let profileUserId = 0;

window.addEventListener("load", Init);

edit_profile_btn.addEventListener("click", ToggleEditMode);

edit_about_btn.addEventListener("click", EnterAboutEdit);
save_about_btn.addEventListener("click", SaveAbout);
close_about_btn.addEventListener("click", CloseAboutEdit);

btn_update_name.addEventListener("click", UpdateUserName);
btn_update_username.addEventListener("click", UpdateUserUsername);
btn_update_contact.addEventListener("click", UpdateUserContact);

async function Init() {
    await UserSession();

    profileUserId = GetProfileUserIdFromUrl();

    await GetProfile();
}

function GetProfileUserIdFromUrl() {
    const parts = window.location.pathname.split("/");
    return parseInt(parts[parts.length - 1]);
}

async function GetProfile() {
    const user = await GetUser();
    const companies = await GetUserCompanies();

    RenderUserProfile(user);
    RenderCompanyCards(companies);
}

async function GetUser() {
    try {
        const response = await fetch(
            `https://localhost:7142/api/User?id=${profileUserId}`
        );

        if (!response.ok) {
            console.log("GET USER — BAD RESPONSE");
            return null;
        }

        return await response.json();
    } catch (e) {
        console.log("GET USER ERROR");
        console.log(e);
        return null;
    }
}

async function GetUserCompanies() {
    try {
        const response = await fetch(
            `https://localhost:7142/api/Company/companies/user?id=${profileUserId}`
        );

        if (!response.ok) {
            console.log("GET COMPANIES — BAD RESPONSE");
            return [];
        }

        return await response.json();
    } catch (e) {
        console.log("GET COMPANIES ERROR");
        console.log(e);
        return [];
    }
}

function RenderUserProfile(data) {
    if (!data) return;

    name_label.innerText = `${data.firstName} ${data.lastName}`;
    username_label.innerText = `@${data.userName}`;
    email_label.innerText = data.email;
    phone_label.innerText = data.phone;

    firstname.value = data.firstName;
    lastname.value = data.lastName;
    username.value = data.userName;
    email.value = data.email;
    phone.value = data.phone;

    about_text.innerText =
        data.about && data.about.length > 0
            ? data.about
            : "No description set yet.";

    about_area.value = data.about ?? "";

    document.getElementById("profile-role").innerText = data.role;

    const isOwner = UserContainer.UserID === profileUserId;

    edit_profile_btn.classList.toggle("hidden", !isOwner);
    edit_about_btn.classList.toggle("hidden", !isOwner);
}

function RenderCompanyCards(data) {
    container.innerHTML = "";

    if (!data || data.length === 0) {
        summary.classList.add("hidden");
        summary.innerText = "";
        check.innerText = "";
        return;
    }

    summary.classList.remove("hidden");
    summary.innerText = `Proud owner of: ${data.map(c => c.companyName).join(", ")}`;
    check.innerText = "";

    data.forEach(element => {
        const template = card_template.content.cloneNode(true);
        const card = template.querySelector(".company-card");

        template.querySelector(".card-owner").textContent = element.ownerName;
        template.querySelector(".card-company").textContent = element.companyName;
        template.querySelector(".card-description").textContent = element.description;
        template.querySelector(".card-button").href = `/Company/${element.id}`;

        card.style.opacity = 0;
        container.appendChild(template);

        setTimeout(() => {
            card.style.opacity = 1;
        }, 1);
    });
}

function ToggleEditMode() {
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
        const response = await fetch(
            `https://localhost:7142/api/User/update/about`,
            {
                method: "PUT",
                headers: { "Content-Type": "application/json" },
                credentials: "include",
                body: JSON.stringify({
                    id: profileUserId,
                    about: about_area.value,
                }),
            }
        );

        if (response.ok) {
            about_text.innerText = about_area.value;
        } else {
            console.log("SAVE ABOUT — BAD RESPONSE");
        }
    } catch (e) {
        console.log("SAVE ABOUT ERROR");
        console.log(e);
    }

    CloseAboutEdit();
}

async function UpdateUserName() {
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
        } else {
            console.log("UPDATE NAME — BAD RESPONSE");
        }
    } catch (e) {
        console.log("UPDATE NAME ERROR");
        console.log(e);
    }
}

async function UpdateUserUsername() {
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
        } else {
            console.log("UPDATE USERNAME — BAD RESPONSE");
        }
    } catch (e) {
        console.log("UPDATE USERNAME ERROR");
        console.log(e);
    }
}

async function UpdateUserContact() {
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
        } else {
            console.log("UPDATE CONTACT — BAD RESPONSE");
        }
    } catch (e) {
        console.log("UPDATE CONTACT ERROR");
        console.log(e);
    }
}

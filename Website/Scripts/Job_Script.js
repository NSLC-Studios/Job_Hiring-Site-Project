let JobId = 0;

const job_company_label = document.getElementById("job-company-label");
const job_pay_label = document.getElementById("job-pay-label");
const job_pay_label_dup = document.getElementById("job-pay-label-dup");
const job_work_label = document.getElementById("job-work-label");
const job_language_label = document.getElementById("job-language-label");
const job_address = document.getElementById("job-address");
const job_address_dup = document.getElementById("job-address-dup");

const job_title_input = document.getElementById("job-title-input");
const job_pay_input = document.getElementById("job-pay-input");
const job_work_input = document.getElementById("job-work-input");
const job_language_input = document.getElementById("job-language-input");
const job_location = document.getElementById("job-location");

const job_pay_group = document.getElementById("job-pay-group");
const job_work_group = document.getElementById("job-work-group");
const job_language_group = document.getElementById("job-language-group");
const job_location_group = document.getElementById("job-location-group");

const edit_job_btn = document.getElementById("edit-job-btn");

const edit_jobdesc_btn = document.getElementById("edit-jobdesc-btn");
const jobdesc_group = document.getElementById("edit-jobdesc-group");
const save_jobdesc_btn = document.getElementById("save-jobdesc-btn");
const close_jobdesc_btn = document.getElementById("close-jobdesc-btn");

const jobdesc_area = document.getElementById("jobdesc-area");
const jobdesc_text = document.getElementById("jobdesc-text");

const apply_btn_top = document.getElementById("apply-btn-top");
const apply_btn_bottom = document.getElementById("apply-btn-bottom");

let edit_mode = false;
let desc_edit_mode = false;

window.addEventListener("load", Init);

edit_job_btn.addEventListener("click", ToggleEditMode);

edit_jobdesc_btn.addEventListener("click", EnterDescEdit);
save_jobdesc_btn.addEventListener("click", SaveDesc);
close_jobdesc_btn.addEventListener("click", CloseDescEdit);

document.getElementById("job-pay-btn").addEventListener("click", UpdateJobPay);
document.getElementById("job-work-btn").addEventListener("click", UpdateJobWork);
document.getElementById("job-language-btn").addEventListener("click", UpdateJobLanguage);
document.getElementById("job-location-btn").addEventListener("click", UpdateJobArea);

async function Init() {
    await UserSession();

    const temp = window.location.pathname.split("/");
    JobId = parseInt(temp[2]);

    await GetJob();
}

async function GetJob() {
    try {
        const response = await fetch(`https://localhost:7142/api/Job?id=${JobId}`);

        if (!response.ok) {
            console.log("GET JOB - BAD RESPONSE");
            return null;
        }

        const data = await response.json();

        job_company_label.innerText = data.companyName;
        job_company_label.href = `/Company/${data.companyID}`;

        job_pay_label.innerText = `${data.pay}€`;
        job_pay_label_dup.innerText = `${data.pay}€`;

        job_work_label.innerText = data.workTime;
        job_language_label.innerText = data.language;

        data.country == null
            ? job_address.innerText = "Job has no address."
            : job_address.innerText = `${data.country}, ${data.county}, ${data.city}, ${data.postal}, ${data.address}`;

        data.country == null
            ? job_address_dup.innerText = "Job has no address."
            : job_address_dup.innerText = `${data.country}, ${data.county}, ${data.city}, ${data.postal}, ${data.address}`;

        jobdesc_text.innerText = data.description && data.description.length > 0
            ? data.description
            : "No description set yet.";

        job_pay_input.value = data.pay;
        job_work_input.value = data.workTime;
        job_language_input.value = data.language;
        jobdesc_area.value = data.description;

        const companyResponse = await fetch(`https://localhost:7142/api/Company/company?id=${data.companyID}`);

        if (!companyResponse.ok) {
            console.log("GET JOB COMPANY - BAD RESPONSE");
        }

        let owner = false;

        if (companyResponse.ok) {
            const companyData = await companyResponse.json();
            owner = UserContainer.UserID == companyData.ownerID;
        }

        edit_job_btn.classList.toggle("hidden", !owner);
        edit_jobdesc_btn.classList.toggle("hidden", !owner);

        apply_btn_top.href = `/Apply/${JobId}`;
        apply_btn_bottom.href = `/Apply/${JobId}`;
        apply_btn_top.classList.remove("hidden");
        apply_btn_bottom.classList.remove("hidden");

    } catch (e) {
        console.log("GET JOB");
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

        job_location.innerHTML = "<option selected value=''>Choose an area...</option>";
        data.forEach(item => {
            const area = document.createElement("option");
            area.value = item.id;
            area.innerText = item.address;

            const tempAddress = job_address_dup.innerText.split(",").pop().trim().replace("...", "").toLowerCase();
            const compareAddress = item.address.split(",").pop().trim().replace("...", "").toLowerCase();

            if (tempAddress.includes(compareAddress) || compareAddress.includes(tempAddress)) area.selected = true;

            job_location.appendChild(area);
        });

    } catch (e) {
        console.log("GET AREA");
        console.log(e);
        return null;
    }
}

async function ToggleEditMode() {
    edit_mode = !edit_mode;

    if (edit_mode) await GetArea();

    job_pay_group.classList.toggle("hidden");
    job_work_group.classList.toggle("hidden");
    job_language_group.classList.toggle("hidden");
    job_location_group.classList.toggle("hidden");

    job_pay_label_dup.classList.toggle("hidden");
    job_work_label.classList.toggle("hidden");
    job_language_label.classList.toggle("hidden");
    job_address_dup.classList.toggle("hidden");

    edit_job_btn.innerText = edit_mode
        ? "Finish Editing"
        : "Edit";
}

function EnterDescEdit() {
    desc_edit_mode = true;

    edit_jobdesc_btn.classList.add("hidden");
    jobdesc_group.classList.remove("hidden");

    jobdesc_text.classList.add("hidden");
    jobdesc_area.classList.remove("hidden");

    jobdesc_area.value = jobdesc_text.innerText;
}

function CloseDescEdit() {
    desc_edit_mode = false;

    edit_jobdesc_btn.classList.remove("hidden");
    jobdesc_group.classList.add("hidden");

    jobdesc_text.classList.remove("hidden");
    jobdesc_area.classList.add("hidden");
}

async function SaveDesc() {
    try {
        const response = await fetch(`https://localhost:7142/api/Job/update/description`, {
            method: "PUT",
            headers: { "Content-Type": "application/json" },
            credentials: "include",
            body: JSON.stringify({
                id: JobId,
                description: jobdesc_area.value
            })
        });

        if (response.ok) {
            jobdesc_text.innerText = jobdesc_area.value;
        } else {
            console.log("SAVE JOB DESCRIPTION - BAD RESPONSE");
        }
    } catch (e) {
        console.log("SAVE JOB DESCRIPTION");
        console.log(e);
    }

    CloseDescEdit();
}

async function UpdateJobArea() {
    try {
        if (job_location.value == "") return;

        const response = await fetch(`https://localhost:7142/api/Job/update/area`, {
            method: "PUT",
            headers: { "Content-Type": "application/json" },
            credentials: "include",
            body: JSON.stringify({
                id: JobId,
                AreaID: job_location.value
            })
        });

        if (response.ok) {
            job_address.innerText = job_location.options[job_location.selectedIndex].text;
            job_address_dup.innerText = job_location.options[job_location.selectedIndex].text;
        } else {
            console.log("UPDATE AREA - BAD RESPONSE");
        }
    } catch (e) {
        console.log("UPDATE AREA");
        console.log(e);
    }
}

async function UpdateJobTitle() {
    try {
        const response = await fetch(`https://localhost:7142/api/Job/update/description`, {
            method: "PUT",
            headers: { "Content-Type": "application/json" },
            credentials: "include",
            body: JSON.stringify({
                id: JobId,
                description: job_title_input.value
            })
        });

        if (response.ok) {
            jobdesc_text.innerText = job_title_input.value;
        } else {
            console.log("UPDATE JOB TITLE - BAD RESPONSE");
        }
    } catch (e) {
        console.log("UPDATE JOB TITLE");
        console.log(e);
    }
}

async function UpdateJobPay() {
    try {
        const response = await fetch(`https://localhost:7142/api/Job/update/pay`, {
            method: "PUT",
            headers: { "Content-Type": "application/json" },
            credentials: "include",
            body: JSON.stringify({
                id: JobId,
                pay: parseInt(job_pay_input.value)
            })
        });

        if (response.ok) {
            job_pay_label.innerText = `${job_pay_input.value}€`;
            job_pay_label_dup.innerText = `${job_pay_input.value}€`;
        } else {
            console.log("UPDATE JOB PAY - BAD RESPONSE");
        }
    } catch (e) {
        console.log("UPDATE JOB PAY");
        console.log(e);
    }
}

async function UpdateJobWork() {
    try {
        const response = await fetch(`https://localhost:7142/api/Job/update/worktime`, {
            method: "PUT",
            headers: { "Content-Type": "application/json" },
            credentials: "include",
            body: JSON.stringify({
                id: JobId,
                workTime: job_work_input.value
            })
        });

        if (response.ok) {
            job_work_label.innerText = job_work_input.value;
        } else {
            console.log("UPDATE JOB WORKTIME - BAD RESPONSE");
        }
    } catch (e) {
        console.log("UPDATE JOB WORKTIME");
        console.log(e);
    }
}

async function UpdateJobLanguage() {
    try {
        const response = await fetch(`https://localhost:7142/api/Job/update/language`, {
            method: "PUT",
            headers: { "Content-Type": "application/json" },
            credentials: "include",
            body: JSON.stringify({
                id: JobId,
                language: job_language_input.value
            })
        });

        if (response.ok) {
            job_language_label.innerText = job_language_input.value;
        } else {
            console.log("UPDATE JOB LANGUAGE - BAD RESPONSE");
        }
    } catch (e) {
        console.log("UPDATE JOB LANGUAGE");
        console.log(e);
    }
}

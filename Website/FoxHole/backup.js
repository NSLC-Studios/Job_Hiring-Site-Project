let JobId = 0;

const job_title_label = document.getElementById("job-title-label");
const job_company_label = document.getElementById("job-company-label");
const job_pay_label = document.getElementById("job-pay-label");
const job_pay_label_dup = document.getElementById("job-pay-label-dup");
const job_work_label = document.getElementById("job-work-label");
const job_language_label = document.getElementById("job-language-label");
const job_address = document.getElementById("job-address");
const job_address_dup = document.getElementById("job-address-dup");

const job_title_group = document.getElementById("job-title-group");
const job_pay_group = document.getElementById("job-pay-group");
const job_work_group = document.getElementById("job-work-group");
const job_language_group = document.getElementById("job-language-group");

const edit_job_btn = document.getElementById("edit-job-btn");

const edit_jobdesc_btn = document.getElementById("edit-jobdesc-btn");
const jobdesc_group = document.getElementById("edit-jobdesc-group");
const save_jobdesc_btn = document.getElementById("save-jobdesc-btn");
const close_jobdesc_btn = document.getElementById("close-jobdesc-btn");

const jobdesc_area = document.getElementById("jobdesc-area");
const jobdesc_text = document.getElementById("jobdesc-text");

const job_title_input = document.getElementById("job-title-input");
const job_pay_input = document.getElementById("job-pay-input");
const job_work_input = document.getElementById("job-work-input");
const job_language_input = document.getElementById("job-language-input");

const apply_btn_top = document.getElementById("apply-btn-top");
const apply_btn_bottom = document.getElementById("apply-btn-bottom");

let job_edit_mode = false;
let jobdesc_edit_mode = false;

window.addEventListener("load", Init);

edit_job_btn.addEventListener("click", ToggleJobEdit);
edit_jobdesc_btn.addEventListener("click", EnterJobDescEdit);
save_jobdesc_btn.addEventListener("click", SaveJobDesc);
close_jobdesc_btn.addEventListener("click", CloseJobDescEdit);

document.getElementById("job-title-btn").addEventListener("click", UpdateJobTitle);
document.getElementById("job-pay-btn").addEventListener("click", UpdateJobPay);
document.getElementById("job-work-btn").addEventListener("click", UpdateJobWork);
document.getElementById("job-language-btn").addEventListener("click", UpdateJobLanguage);

async function Init() {
    await UserSession();
    const temp = window.location.pathname.split("/");
    JobId = parseInt(temp[2]);
    await GetJob();
}

async function GetJob() {
    try {
        const response = await fetch(`https://localhost:7142/api/Job?id=${JobId}`);
        if (!response.ok) return;

        const data = await response.json();

        job_title_label.innerText = data.description;
        job_company_label.innerText = data.companyName;
        job_company_label.href = `/Company/${data.companyID}`;

        job_pay_label.innerText = `${data.pay}€`;
        job_pay_label_dup.innerText = `${data.pay}€`;

        job_work_label.innerText = data.workTime;
        job_language_label.innerText = data.language;

        if (data.country == null) {
            job_address.innerText = "Job has no address.";
            job_address_dup.innerText = "Job has no address.";
        } else {
            const addr = `${data.country}, ${data.county}, ${data.city}`;
            job_address.innerText = addr;
            job_address_dup.innerText = addr;
        }

        jobdesc_text.innerText = data.description && data.description.length > 0
            ? data.description
            : "No description set yet.";

        job_title_input.value = data.description;
        job_pay_input.value = data.pay;
        job_work_input.value = data.workTime;
        job_language_input.value = data.language;
        jobdesc_area.value = data.description;

        const owner = UserContainer.UserID == data.companyID;
        edit_job_btn.classList.toggle("hidden", !owner);
        edit_jobdesc_btn.classList.toggle("hidden", !owner);

        apply_btn_top.href = `/Apply/${JobId}`;
        apply_btn_bottom.href = `/Apply/${JobId}`;
        apply_btn_top.classList.remove("hidden");
        apply_btn_bottom.classList.remove("hidden");

    } catch { }
}

function ToggleJobEdit() {
    job_edit_mode = !job_edit_mode;

    job_title_group.classList.toggle("hidden");
    job_pay_group.classList.toggle("hidden");
    job_work_group.classList.toggle("hidden");
    job_language_group.classList.toggle("hidden");

    job_title_label.classList.toggle("hidden");
    job_pay_label.classList.toggle("hidden");
    job_work_label.classList.toggle("hidden");
    job_language_label.classList.toggle("hidden");

    edit_job_btn.innerText = job_edit_mode
        ? "Finish Editing"
        : "Edit";
}

function EnterJobDescEdit() {
    jobdesc_edit_mode = true;

    edit_jobdesc_btn.classList.add("hidden");
    jobdesc_group.classList.remove("hidden");

    jobdesc_text.classList.add("hidden");
    jobdesc_area.classList.remove("hidden");

    jobdesc_area.value = jobdesc_text.innerText;
}

function CloseJobDescEdit() {
    jobdesc_edit_mode = false;

    edit_jobdesc_btn.classList.remove("hidden");
    jobdesc_group.classList.add("hidden");

    jobdesc_text.classList.remove("hidden");
    jobdesc_area.classList.add("hidden");
}

async function SaveJobDesc() {
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
        }
    } catch { }

    CloseJobDescEdit();
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
            job_title_label.innerText = job_title_input.value;
        }
    } catch { }
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
        }
    } catch { }
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
        }
    } catch { }
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
        }
    } catch { }
}

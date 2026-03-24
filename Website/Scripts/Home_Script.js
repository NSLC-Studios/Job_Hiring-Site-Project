const card_template = document.getElementById("card-template");
const container = document.getElementById("job-container");
const check = document.getElementById("job-check");

const reg_word = document.getElementById("reg-word");
const reg_search = document.getElementById("reg-search");
const fil_pay = document.getElementById("fil-pay");
const fil_hour = document.getElementById("fil-hour");
const fil_country = document.getElementById("fil-country");
const fil_county = document.getElementById("fil-county");
const fil_city = document.getElementById("fil-city");
const fil_language = document.getElementById("fil-language");
const fil_company = document.getElementById("fil-company");
const fil_search = document.getElementById("fil-search");
const ref_btn = document.getElementById("ref-btn");

reg_search.addEventListener("click", GetSearch);
fil_search.addEventListener("click", GetFilter);
ref_btn.addEventListener("click", GetJobs);
reg_word.addEventListener("keyup", Rerouted);

const page_tab = document.getElementById("page-tab");
const page_back = document.getElementById("page-back");
const page_counter = document.getElementById("page-counter");
const page_forward = document.getElementById("page-forward");

window.addEventListener("load", GetJobs);

function Rerouted() { // e
    reg_search.click();

    /*if (e.key === "Enter"){
        reg_search.click();
    }*/
}

async function GetJobs(event, skip = 0, take = 12) {
    try{
        const response = await fetch(`https://localhost:7142/api/Job/jobs?skip=${skip}&take=${take}`);

        if (response.ok){
            const data = await response.json();

            page_tab.classList.remove("hidden");

            if (event != null) {
                page_counter.innerText = 1;
                page_counter.index = 1;
            }
            
            if (data.length == 0){
                if (event != null) {
                    page_tab.classList.add("hidden");
                }
                
                container.innerHTML = "";
                check.innerText = "No Jobs found!";
                return;
            }

            if (data.length < 12 && event != null){
                page_tab.classList.add("hidden");
            }

            check.innerText = "";
            container.innerHTML = "";

            data.forEach(element => {
                const template = card_template.content.cloneNode(true);
                const card = template.querySelector(".job-card");

                template.querySelector(".card-company").textContent = element.companyName;
                template.querySelector(".card-company").href = `/Company/${element.companyID}`;
                template.querySelector(".card-description").textContent = element.description;
                template.querySelector(".card-pay").textContent = `€${element.pay}`;
                template.querySelector(".card-language").textContent = element.language;
                template.querySelector(".card-workhour").textContent = element.workTime;
                template.querySelector(".card-workhour").textContent = element.workTime;
                template.querySelector(".card-button").href = `/Job/${element.id}`;
                template.querySelector(".card-address").textContent = `${element.country}, ${element.county}, ${element.city}.`;
                
                card.style.opacity = 0;
                container.appendChild(template);
                setTimeout(() => {
                    card.style.opacity = 1;
                }, 1);
            });
        } else{
            if (event != null) {
                page_counter.classList.add("hidden");
            }

            container.innerHTML = "";
            check.innerText = "No Jobs found!";
        }
    } catch (e){
        console.log("LOAD JOBS");
        console.log(e);
    }
}

async function GetSearch(event, skip = 0, take = 12) {
    try{
        const response = await fetch(`https://localhost:7142/api/Job/jobs/search?description=${reg_word.value}&skip=${skip}&take=${take}`);

        if (response.ok){
            const data = await response.json();

            page_tab.classList.remove("hidden");

            if (event != null) {
                page_counter.innerText = 1;
                page_counter.index = 1;
            }

            if (data.length == 0){
                if (event != null) {
                    page_tab.classList.add("hidden");
                }

                container.innerHTML = "";
                check.innerText = "No Jobs found!";
                return;
            }

            if (data.length < 12 && event != null){
                page_tab.classList.add("hidden");
            }

            if (event != null) {
                page_counter.innerText = 1;
            }

            check.innerText = "";
            container.innerHTML = "";

            data.forEach(element => {
                const template = card_template.content.cloneNode(true);
                const card = template.querySelector(".job-card");

                template.querySelector(".card-company").textContent = element.companyName;
                template.querySelector(".card-company").href = `/Company/${element.companyID}`;
                template.querySelector(".card-description").textContent = element.description;
                template.querySelector(".card-pay").textContent = element.pay;
                template.querySelector(".card-language").textContent = element.language;
                template.querySelector(".card-workhour").textContent = element.workTime;
                template.querySelector(".card-workhour").textContent = element.workTime;
                template.querySelector(".card-button").href = `/Job/${element.id}`;
                template.querySelector(".card-address").textContent = `${element.country}, ${element.county}, ${element.city}.`;
                
                card.style.opacity = 0;
                container.appendChild(template);
                setTimeout(() => {
                    card.style.opacity = 1;
                }, 1);
            });
        } else{
            if (event != null) {
                page_counter.classList.add("hidden");
            }

            container.innerHTML = "";
            check.innerText = "No Jobs found!";
        }
    } catch (e){
        console.log("SEARCH JOBS");
        console.log(e);
    }
}

async function GetFilter(event, skip = 0, take = 12) {
    try{
        const response = await fetch(`https://localhost:7142/api/Job/jobs/filter?pay=${fil_pay.value}&language=${fil_language.value}&country=${fil_country.value}&county=${fil_county.value}&city=${fil_city.value}&work=${fil_hour.value}&company=${fil_company.value}&description=${reg_word.value}&skip=${skip}&take=${take}`);

        if (response.ok){
            const data = await response.json();

            page_tab.classList.remove("hidden");

            if (event != null) {
                page_counter.innerText = 1;
                page_counter.index = 1;
            }

            if (data.length == 0){
                if (event != null) {
                    page_tab.classList.add("hidden");
                }

                container.innerHTML = "";
                check.innerText = "No Jobs found!";
                return;
            }

            if (data.length < 12 && event != null){
                page_tab.classList.add("hidden");
            }

            if (event != null) {
                page_counter.innerText = 1;
            }

            check.innerText = "";
            container.innerHTML = "";

            data.forEach(element => {
                const template = card_template.content.cloneNode(true);
                const card = template.querySelector(".job-card");

                template.querySelector(".card-company").textContent = element.companyName;
                template.querySelector(".card-company").href = `/Company/${element.companyID}`;
                template.querySelector(".card-description").textContent = element.description;
                template.querySelector(".card-pay").textContent = element.pay;
                template.querySelector(".card-language").textContent = element.language;
                template.querySelector(".card-workhour").textContent = element.workTime;
                template.querySelector(".card-workhour").textContent = element.workTime;
                template.querySelector(".card-button").href = `/Job/${element.id}`;
                template.querySelector(".card-address").textContent = `${element.country}, ${element.county}, ${element.city}.`;
                
                card.style.opacity = 0;
                container.appendChild(template);
                setTimeout(() => {
                    card.style.opacity = 1;
                }, 1);
            });
        } else{
            if (event != null) {
                page_counter.classList.add("hidden");
            }

            container.innerHTML = "";
            check.innerText = "No Jobs found!";
        }
    } catch (e){
        console.log("SEARCH JOBS");
        console.log(e);
    }
}

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

window.addEventListener("load", GetJobs);

reg_search.addEventListener("click", GetSearch);

async function GetJobs() {
    try{
        const response = await fetch("https://localhost:7142/api/Job/jobs?skip=0&take=12");

        if (response.ok){
            const data = await response.json();

            if (data.length == 0){
                container.innerHTML = "";
                check.innerText = "No Jobs found!";
                return;
            }

            check.innerText = "";
            container.innerHTML = "";

            data.forEach(element => {
                const template = card_template.content.cloneNode(true);

                template.querySelector(".card-company").textContent = element.companyName;
                template.querySelector(".card-company").href = `/Company/${element.companyID}`;
                template.querySelector(".card-description").textContent = element.description;
                template.querySelector(".card-pay").textContent = `€${element.pay}`;
                template.querySelector(".card-language").textContent = element.language;
                template.querySelector(".card-workhour").textContent = element.workTime;
                template.querySelector(".card-workhour").textContent = element.workTime;
                template.querySelector(".card-button").href = `/Job/${element.id}`;
                template.querySelector(".card-address").textContent = `${element.country}, ${element.county}, ${element.city}.`;

                container.appendChild(template);
            });
        } else{
            container.innerHTML = "";
            check.innerText = "No Jobs found!";
        }
    } catch (e){
        console.log("LOAD JOBS");
        console.log(e);
    }
}

async function GetSearch() {
    try{
        const response = await fetch(`https://localhost:7142/api/Job/jobs/search?description=${reg_word.value}&skip=0&take=12`);

        if (response.ok){
            const data = await response.json();

            if (data.length == 0){
                container.innerHTML = "";
                check.innerText = "No Jobs found!";
                return;
            }

            check.innerText = "";
            container.innerHTML = "";

            data.forEach(element => {
                const template = card_template.content.cloneNode(true);

                template.querySelector(".card-company").textContent = element.companyName;
                template.querySelector(".card-company").href = `/Company/${element.companyID}`;
                template.querySelector(".card-description").textContent = element.description;
                template.querySelector(".card-pay").textContent = element.pay;
                template.querySelector(".card-language").textContent = element.language;
                template.querySelector(".card-workhour").textContent = element.workTime;
                template.querySelector(".card-workhour").textContent = element.workTime;
                template.querySelector(".card-button").href = `/Job/${element.id}`;
                template.querySelector(".card-address").textContent = `${element.country}, ${element.county}, ${element.city}.`;
                
                container.appendChild(template);
            });
        } else{
            container.innerHTML = "";
            check.innerText = "No Jobs found!";
        }
    } catch (e){
        console.log("SEARCH JOBS");
        console.log(e);
    }
}

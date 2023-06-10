const viaCEPBaseUrl = "https://viacep.com.br/ws";
const UrlSuffix = "json";
const cepInput = document.getElementById("Input_CEP");
const cepErrorSpan = document.getElementById("cep-error-span");
const logradouroInput = document.getElementById("Input_Logradouro")
const complementoInput = document.getElementById("Input_Complemento")
const cidadeInput = document.getElementById("Input_Cidade")
const bairroInput = document.getElementById("Input_Bairro")
const estadoInput = document.getElementById("Input_Estado")
const cepSpinner = document.getElementById("cep-spinner");
const submitSpinner = document.getElementById("submit-spinner");
const form = document.getElementById("registerForm");
const submitBtn = document.getElementById("registerSubmit");

cepInput.addEventListener("keyup", async e => {
    const valid = validar(e.target.value);
    cepErrorSpan.innerHTML = "";
    if (!valid) {
        cepErrorSpan.innerHTML = "CEP inválido";
        limparCampos();
        return;
    }

    const numeros = e.target.value.replace(/\D/g, '');
    await getEndereco(numeros);
});

form.addEventListener("submit", () => {
    submitSpinner.classList.remove("d-none");
    if (checkErrors) return;

    submitBtn.disabled = true;
    submitBtn.childNodes[3].classList.add("d-none");
    submitSpinner.classList.add("d-none");
});

const validar = val => {
    const temLetras = /[a-zA-Z]/g.test(val);
    const tamanhoCorreto = val.replace(/\D/g, '').length === 8;

    return !temLetras && tamanhoCorreto;
}

const checkErrors = () => {
    const errors = document.querySelectorAll(".field-validation-error");
    return errors.length > 0;
}

const getEndereco = async cep => {
    cepSpinner.classList.remove("d-none");
    const response = await fetch(`${viaCEPBaseUrl}/${cep}/${UrlSuffix}`);
    const result = await response.text();
    let resultParsed = JSON.parse(result);

    if (resultParsed.erro) {
        cepErrorSpan.innerHTML = "CEP não encontrado";
        limparCampos();
        cepSpinner.classList.add("d-none");
        return;
    }

    logradouroInput.value = resultParsed.logradouro;
    complementoInput.value = resultParsed.complemento;
    cidadeInput.value = resultParsed.localidade;
    bairroInput.value = resultParsed.bairro;
    estadoInput.value = resultParsed.uf;
    cepSpinner.classList.add("d-none");
}

const limparCampos = () => {
    logradouroInput.value = "";
    complementoInput.value = "";
    cidadeInput.value = "";
    bairroInput.value = "";
    estadoInput.value = "";
}
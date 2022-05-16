var surgy = 1;
var Manifestations = 1;
var History = 1;
var Medicine = 1;

function hide(id) {
        document.getElementById(id).style.display = "none";
}

function Show(id) {
        document.getElementById(id).style.display = 'block';
    document.getElementById(id).style.display = 'table';
}

function addMedicine(id) {
    var markup = `<tr>
         <td><input type="text" class="form-control" asp-for="Medicine[` + Medicine + `].Medicine" /></td>
         <td><input type="text" class="form-control" asp-for="Medicine[` + Medicine + `].Dose" /></td>
         <td><input type="text" class="form-control" asp-for="Medicine[` + Medicine + `].Frequency" /></td>
         <td><input type="text" class="form-control" asp-for="Medicine[` + Medicine + `].ReasonIfKnown" /></td>
         <td><input type="checkbox" class="form-check-input" asp-for="Medicine[` + Medicine + `].Continue" /></td>
         <td>
            <button type="button" class="btn btn-outline-danger" onclick='Remove(this)'>
                <i class="flaticon-delete"></i>
            </button>
            <input type="hidden" asp-for="Medicine[` + Medicine + `].MedicineID" />
        </td>
</tr>`;
    $(id).append(markup);
}

function addsurgy(id) {
        var markup = `<tr>
        <td><input type='text' class='form-control' asp-for='SurgialHistory[` + surgy + `].Procedure' /></td>
        <td><input type='text' class='form-control' asp-for='SurgialHistory[` + surgy + `].Details' /></td>
        <td>
            <button type="button" class="btn btn-outline-danger" onclick='Remove(this)'>
                <i class="flaticon-delete"></i>
            </button>
            <input type="hidden" asp-for="SurgialHistory[` + surgy + `].ID" />
        </td></tr>`;
$(id).append(markup);
}

function addHisttory(id) {
        var markup = `<tr>
        <td><input type='text' class='form-control' asp-for='PastHistory[` + History + `].Problem' /></td>
        <td><input type='text' class='form-control' asp-for='PastHistory[` + History + `].Details' /></td>
        <td>
            <button type="button" class="btn btn-outline-danger" onclick='Remove(this)'>
                <i class="flaticon-delete"></i>
            </button>
            <input type="hidden" asp-for="PastHistory[` + History + `].ID" />
        </td></tr>`;
$(id).append(markup);
}

function addManifestations(id) {
        var markup = `<tr>
        <td><input type='text' class='form-control' asp-for='Allergies[` + Manifestations + `].Name' /></td>
        <td><input type='text' class='form-control' asp-for='Allergies[` + Manifestations + `].ReactionManifestations' /></td>
        <td>
            <button type="button" class="btn btn-outline-danger" onclick='Remove(this)'>
                <i class="flaticon-delete"></i>
            </button>
            <input type="hidden" asp-for="Allergies[` + Manifestations + `].ID" />
        </td></tr>`;
$(id).append(markup);
}

function Remove(td) {
    $(td).parent().parent().remove();
}
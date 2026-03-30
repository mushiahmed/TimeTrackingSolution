var apiUrl = window.apiBaseUrl + "timeentries"; //"/api/timeentries";

function getFormData(isCreate) {
    return {
        id: !isCreate ? $("#entryId").val() : 0,
        employeeId: $("#employeeId").val(),
        projectId: $("#projectId").val(),
        entryDate: $("#entryDate").val(),
        hours: $("#hours").val(),
        notes: $("#notes").val(),
        source: $("#source").val()
    };
}

function createEntry() {
    var entry = getFormData(true);

    $.ajax({
        url: apiUrl,
        type: "POST",
        data: JSON.stringify(entry),
        contentType: "application/json",
        success: function () {
            alert("Time Entry Created");
        },
        error: function () {
            alert("Error saving record");
        }
    });
}

function updateEntry() {
    var entry = getFormData(false);

    $.ajax({
        url: apiUrl + "/" + entry.id,
        type: "PUT",
        data: JSON.stringify(entry),
        contentType: "application/json",
        success: function () {
            alert("Time Entry Updated");
        },
        error: function () {
            alert("Error saving record");
        }
    });
}

function deleteEntry() {
    var id = $("#entryId").val();

    $.ajax({
        url: apiUrl + "/" + id,
        type: "DELETE",
        success: function () {
            alert("Time Entry Deleted");
        },
        error: function () {
            alert("Error deleting record");
        }
    });
}
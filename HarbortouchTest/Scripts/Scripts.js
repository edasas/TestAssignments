var Harbortouch = {
    RegisterSearch: function (inputName, tableId, tableColumnClassName) {
        if (Harbortouch.registeredColumns.indexOf(inputName) == -1) {
            Harbortouch.registeredColumns.push(inputName);

            document.getElementById(inputName).onkeyup = function (e) {

                Harbortouch.Search(inputName, tableId, tableColumnClassName);
            }
        }
    },
    registeredColumns: [],
    searchItems: [],
    searchItem: function (inputName, input, searchText, tableColumnClassName) {
        this.inputName = inputName;
        this.input = input;
        this.searchText = searchText;
        this.tableColumnClassName = tableColumnClassName;
    },
    Search: function (inputName, tableId, tableColumnClassName) {

        var input = document.getElementById(inputName);
        var filteringText = input.value.toLowerCase();

        var searchItemAlreadyRegistered = false;
        for (i = 0; i < Harbortouch.searchItems.length; i++) {
            if (Harbortouch.searchItems[i].inputName === inputName) {
                Harbortouch.searchItems[i].searchText = filteringText.toLowerCase();
                searchItemAlreadyRegistered = true;
                break;
            }
        }

        if (!searchItemAlreadyRegistered) {
            Harbortouch.searchItems.push(new Harbortouch.searchItem(inputName, input, filteringText.toLowerCase(), tableColumnClassName));
        }

        Harbortouch.UpdateSearchResults(tableId);


    },
    UpdateSearchResults: function (tableId) {
        var table = document.getElementById(tableId);
        var tableRows = table.getElementsByClassName("model-line");

        var displayStyle = "";

        for (i = 0; i < tableRows.length; i++) {
            displayStyle = "";

            for (j = 0; j < Harbortouch.searchItems.length; j++) {
                var filterableColumn = tableRows[i].getElementsByClassName(Harbortouch.searchItems[j].tableColumnClassName);

                if (filterableColumn && Harbortouch.searchItems[j].searchText != "") {
                    if (filterableColumn[0].innerText.toLowerCase().indexOf(Harbortouch.searchItems[j].searchText) == -1) {
                        displayStyle = "none";
                        break;
                    }
                }
            }
            tableRows[i].style.display = displayStyle;
        }
    }
}
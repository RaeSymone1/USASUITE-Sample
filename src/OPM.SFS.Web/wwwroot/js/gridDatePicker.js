﻿class DatePickerMMYYYY {
    // gets called once before the renderer is used
    //https://www.ag-grid.com/javascript-data-grid/cell-editors/#datepicker-cell-editing-example
    init(params) {
        // create the cell
        this.eInput = document.createElement('input');
        this.eInput.value = params.value;
        this.eInput.classList.add('ag-input');
        this.eInput.style.height = 'var(--ag-row-height)';
        this.eInput.style.fontSize = 'calc(var(--ag-font-size) + 1px)';

        // https://jqueryui.com/datepicker/
        //https://api.jqueryui.com/datepicker/#utility-formatDate
        $(this.eInput).datepicker({
            dateFormat: 'M yy',
            onSelect: () => {
                this.eInput.focus();
            },
        });
    }

    // gets called once when grid ready to insert the element
    getGui() {
        return this.eInput;
    }

    // focus and select can be done after the gui is attached
    afterGuiAttached() {
        this.eInput.focus();
        this.eInput.select();
    }

    // returns the new value after editing
    getValue() {
        return this.eInput.value;
    }

    // any cleanup we need to be done here
    destroy() {
        // but this example is simple, no cleanup, we could
        // even leave this method out as it's optional
    }

    // if true, then this editor will appear in a popup
    isPopup() {
        // and we could leave this method out also, false is the default
        return false;
    }
}

class DatePicker {
    // gets called once before the renderer is used
    //https://www.ag-grid.com/javascript-data-grid/cell-editors/#datepicker-cell-editing-example
    init(params) {
        // create the cell
        this.eInput = document.createElement('input');
        this.eInput.value = params.value;
        this.eInput.classList.add('ag-input');
        this.eInput.style.height = 'var(--ag-row-height)';
        this.eInput.style.fontSize = 'calc(var(--ag-font-size) + 1px)';

        // https://jqueryui.com/datepicker/
        //https://api.jqueryui.com/datepicker/#utility-formatDate
        $(this.eInput).datepicker({
            dateFormat: 'mm/dd/yy',
            onSelect: () => {
                this.eInput.focus();
            },
        });
    }

    // gets called once when grid ready to insert the element
    getGui() {
        return this.eInput;
    }

    // focus and select can be done after the gui is attached
    afterGuiAttached() {
        this.eInput.focus();
        this.eInput.select();
    }

    // returns the new value after editing
    getValue() {
        return this.eInput.value;
    }

    // any cleanup we need to be done here
    destroy() {
        // but this example is simple, no cleanup, we could
        // even leave this method out as it's optional
    }

    // if true, then this editor will appear in a popup
    isPopup() {
        // and we could leave this method out also, false is the default
        return false;
    }
}

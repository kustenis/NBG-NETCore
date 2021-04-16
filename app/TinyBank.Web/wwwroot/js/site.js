// Please see documentation at https://docs.microsoft.com/aspnet/core/client-side/bundling-and-minification
// for details on configuring this project to bundle and minify static web assets.

// Write your JavaScript code.

$('.js-update-customer').on('click',
    (event) => {
        debugger;
        let firstName = $('.js-first-name').val();
        let lastName = $('.js-last-name').val();
        let customerId = $('.js-customer-id').val();

        console.log(`${firstName} ${lastName}`);

        let data = JSON.stringify({
            firstName: firstName,
            lastName: lastName
        });

        // ajax call
        let result = $.ajax({
            url: `/customer/${customerId}`,
            method: 'PUT',
            contentType: 'application/json',
            data: data
        }).done(response => {
            console.log('Update was successful');
            // success
        }).fail(failure => {
            // fail
            console.log('Update failed');
        });
    });

$('.js-customers-list tbody tr').on('click',
    (event) => {
        console.log($(event.currentTarget).attr('id'));
    });

$('.js-card-checkout-cardNumber-pay').on('click',
    (event) => {
        debugger;

        let alertSuccess = $('#js-card-checkout-success').hide();
        let alertRejected = $('#js-card-checkout-error').hide();
        let checkoutForm = $('#js-card-checkout-form');

        let cardNumber = $('.js-card-checkout-cardNumber').val();
        let expirationMonth = $('.js-card-checkout-expirationMonth').val();
        let expirationYear = $('.js-card-checkout-expirationYear').val();
        let amount = $('.js-card-checkout-amount').val();

        let data = JSON.stringify({
            cardNumber: cardNumber,
            expirationMonth: expirationMonth,
            expirationYear: expirationYear,
            amount: amount
        });


        ActivateDeactivateForm("js-card-checkout-form", true);

        // ajax call
        let result = $.ajax({
            url: '/card/checkout',
            method: 'POST',
            contentType: 'application/json',
            data: data
        }).done(response => {
            console.log('Update was successful');
            checkoutForm.hide();
            alertSuccess.show();
        }).fail(failure => {
            console.log('Update failed');
            alertRejected.show();
        }).always(() => { ActivateDeactivateForm("js-card-checkout-form", false); });
    });

function ActivateDeactivateForm(formId, enable) {
    if (!formId) return;
    $(`#${formId} :input`).prop("disabled", enable);
}
/**
 * Flexible Product Configurator with Real-time Cascading
 * Implements rule-based attribute cascading and dynamic pricing
 */
window.FlexibleConfigurator = (function() {
    'use strict';

    let config = {};
    let currentConfiguration = {};
    let isUpdating = false;
    let updateTimeout = null;

    // Initialize the configurator
    function init(options) {
        config = Object.assign({
            productId: 0,
            configurableProductId: 0,
            basePrice: 0,
            primaryUnit: 'm²',
            updateDelay: 300, // milliseconds
            endpoints: {},
            labels: {}
        }, options);

        bindEvents();
        initializeConfiguration();
        updateConfiguration(true); // Initial update
    }

    // Bind event handlers
    function bindEvents() {
        // Attribute change handlers
        $(document).on('change', '.option-input, .checkbox-input, .numeric-input, .text-input, .toggle-input', function() {
            if (!isUpdating) {
                debounceUpdate();
            }
        });

        // Option card click handlers
        $(document).on('click', '.option-card', function() {
            const $card = $(this);
            const $input = $card.find('.option-input');
            
            if (!$card.hasClass('disabled')) {
                $input.prop('checked', true);
                updateOptionSelection($card.closest('.attribute-section'));
                debounceUpdate();
            }
        });

        // Action button handlers
        $('#save-config-btn').on('click', saveConfiguration);
        $('#generate-quote-btn').on('click', generateQuote);
        $('#add-to-cart-btn').on('click', addToCart);
    }

    // Initialize configuration from current form state
    function initializeConfiguration() {
        $('.attribute-section').each(function() {
            const $section = $(this);
            const attributeName = $section.data('attribute');
            const inputType = $section.data('input-type');
            
            let value = getAttributeValue($section, inputType);
            if (value !== null && value !== undefined && value !== '') {
                currentConfiguration[attributeName] = value;
            }
        });
    }

    // Get the current value of an attribute
    function getAttributeValue($section, inputType) {
        const attributeName = $section.data('attribute');
        
        switch (inputType) {
            case 'SingleSelect':
                const $selectedOption = $section.find('.option-input:checked');
                return $selectedOption.length ? $selectedOption.val() : null;
                
            case 'MultiSelect':
                const selectedValues = [];
                $section.find('.checkbox-input:checked').each(function() {
                    selectedValues.push($(this).val());
                });
                return selectedValues.length ? selectedValues : null;
                
            case 'Numeric':
                const numValue = $section.find('.numeric-input').val();
                return numValue !== '' ? parseFloat(numValue) : null;
                
            case 'Text':
                return $section.find('.text-input').val() || null;
                
            case 'Boolean':
                return $section.find('.toggle-input').is(':checked');
                
            default:
                return null;
        }
    }

    // Debounced update to prevent excessive API calls
    function debounceUpdate() {
        if (updateTimeout) {
            clearTimeout(updateTimeout);
        }
        updateTimeout = setTimeout(() => updateConfiguration(), config.updateDelay);
    }

    // Main configuration update function
    async function updateConfiguration(isInitial = false) {
        if (isUpdating && !isInitial) return;
        
        isUpdating = true;
        showLoading();

        try {
            // Update current configuration from form
            if (!isInitial) {
                updateCurrentConfigurationFromForm();
            }

            // Get available options based on current configuration
            await updateAvailableOptions();

            // Validate configuration
            await validateConfiguration();

            // Calculate price
            await calculatePrice();

            // Update UI
            updateConfigurationSummary();

        } catch (error) {
            console.error('Configuration update failed:', error);
            showError('Failed to update configuration: ' + error.message);
        } finally {
            isUpdating = false;
            hideLoading();
        }
    }

    // Update current configuration from form values
    function updateCurrentConfigurationFromForm() {
        const newConfiguration = {};
        
        $('.attribute-section').each(function() {
            const $section = $(this);
            const attributeName = $section.data('attribute');
            const inputType = $section.data('input-type');
            
            let value = getAttributeValue($section, inputType);
            if (value !== null && value !== undefined && value !== '') {
                newConfiguration[attributeName] = value;
            }
        });
        
        currentConfiguration = newConfiguration;
    }

    // Get available options based on current configuration
    async function updateAvailableOptions() {
        const attributeNames = [];
        $('.attribute-section').each(function() {
            attributeNames.push($(this).data('attribute'));
        });

        const response = await $.ajax({
            url: config.endpoints.getAvailableOptions,
            type: 'POST',
            contentType: 'application/json',
            data: JSON.stringify({
                configurableProductId: config.configurableProductId,
                attributeNames: attributeNames,
                currentConfiguration: currentConfiguration
            })
        });

        if (response.success) {
            updateAttributeAvailability(response.availableOptions);
        } else {
            throw new Error(response.message || 'Failed to get available options');
        }
    }

    // Update attribute option availability based on rules
    function updateAttributeAvailability(availableOptions) {
        $('.attribute-section').each(function() {
            const $section = $(this);
            const attributeName = $section.data('attribute');
            const options = availableOptions[attributeName] || [];
            
            if (options.length > 0) {
                // Update option availability
                $section.find('.option-card').each(function() {
                    const $card = $(this);
                    const optionId = parseInt($card.data('option-id'));
                    const isAvailable = options.some(opt => opt.id === optionId);
                    
                    $card.toggleClass('disabled', !isAvailable);
                    $card.find('.option-input').prop('disabled', !isAvailable);
                    
                    if (!isAvailable && $card.hasClass('selected')) {
                        // Deselect unavailable options
                        $card.removeClass('selected');
                        $card.find('.option-input').prop('checked', false);
                    }
                });
                
                // Update option data (technical properties, etc.)
                options.forEach(option => {
                    const $card = $section.find(`[data-option-id="${option.id}"]`);
                    if ($card.length) {
                        updateOptionData($card, option);
                    }
                });
            }
        });
    }

    // Update option data with technical information
    function updateOptionData($card, optionData) {
        // Update technical data display if needed
        if (optionData.technicalData) {
            // Could add tooltips or expandable sections with technical data
        }
        
        if (optionData.materialProperties) {
            // Could update material property displays
        }
    }

    // Validate current configuration
    async function validateConfiguration() {
        const response = await $.ajax({
            url: config.endpoints.validateConfiguration,
            type: 'POST',
            contentType: 'application/json',
            data: JSON.stringify({
                configurableProductId: config.configurableProductId,
                configuration: currentConfiguration
            })
        });

        if (response.success) {
            displayValidationResults(response);
        } else {
            throw new Error(response.message || 'Validation failed');
        }
    }

    // Display validation results
    function displayValidationResults(validationData) {
        const $messages = $('#validation-messages');
        const $errors = $('#validation-errors');
        const $warnings = $('#validation-warnings');
        
        $errors.empty();
        $warnings.empty();
        
        if (validationData.errors && validationData.errors.length > 0) {
            validationData.errors.forEach(error => {
                $errors.append(`<div class="validation-error">${error}</div>`);
            });
            $messages.show();
        }
        
        if (validationData.warnings && validationData.warnings.length > 0) {
            validationData.warnings.forEach(warning => {
                $warnings.append(`<div class="validation-warning">${warning}</div>`);
            });
            $messages.show();
        }
        
        if ((!validationData.errors || validationData.errors.length === 0) && 
            (!validationData.warnings || validationData.warnings.length === 0)) {
            $messages.hide();
        }
    }

    // Calculate price breakdown
    async function calculatePrice() {
        const response = await $.ajax({
            url: config.endpoints.calculatePrice,
            type: 'POST',
            contentType: 'application/json',
            data: JSON.stringify({
                configurableProductId: config.configurableProductId,
                configuration: currentConfiguration
            })
        });

        if (response.success) {
            // Update configuration if auto-set rules were applied
            if (response.updatedConfiguration) {
                updateFormFromConfiguration(response.updatedConfiguration);
                currentConfiguration = response.updatedConfiguration;
            }
            
            updatePriceDisplay(response.priceBreakdown, response.dimensions);
        } else {
            throw new Error(response.message || 'Price calculation failed');
        }
    }

    // Update form from configuration (for auto-set rules)
    function updateFormFromConfiguration(configuration) {
        Object.keys(configuration).forEach(attributeName => {
            const $section = $(`.attribute-section[data-attribute="${attributeName}"]`);
            if ($section.length) {
                const inputType = $section.data('input-type');
                const value = configuration[attributeName];
                
                setAttributeValue($section, inputType, value);
            }
        });
    }

    // Set attribute value in the form
    function setAttributeValue($section, inputType, value) {
        switch (inputType) {
            case 'SingleSelect':
                $section.find('.option-input').prop('checked', false);
                $section.find('.option-card').removeClass('selected');
                
                const $targetInput = $section.find(`.option-input[value="${value}"]`);
                if ($targetInput.length) {
                    $targetInput.prop('checked', true);
                    $targetInput.closest('.option-card').addClass('selected');
                }
                break;
                
            case 'Numeric':
                $section.find('.numeric-input').val(value);
                break;
                
            case 'Text':
                $section.find('.text-input').val(value);
                break;
                
            case 'Boolean':
                $section.find('.toggle-input').prop('checked', value === true || value === 'true');
                break;
        }
    }

    // Update price display
    function updatePriceDisplay(priceBreakdown, dimensions) {
        // Update main price components
        updatePriceRow('#base-price', priceBreakdown.basePrice);
        updatePriceRow('#material-cost', priceBreakdown.materialCost);
        updatePriceRow('#area-cost', priceBreakdown.areaBasedCost);
        updatePriceRow('#perimeter-cost', priceBreakdown.perimeterBasedCost);
        updatePriceRow('#hole-cost', priceBreakdown.holeCost);
        updatePriceRow('#process-cost', priceBreakdown.processCost);
        updatePriceRow('#finishing-cost', priceBreakdown.finishingCost);
        updatePriceRow('#setup-fees', priceBreakdown.setupFees);
        
        // Update total with animation
        const $totalPrice = $('#total-price');
        const currentTotal = parseFloat($totalPrice.text().replace(/[^0-9.-]+/g, ""));
        const newTotal = priceBreakdown.totalPrice;
        
        if (Math.abs(currentTotal - newTotal) > 0.01) {
            $totalPrice.addClass('price-updated');
            setTimeout(() => $totalPrice.removeClass('price-updated'), 1000);
        }
        
        $totalPrice.text(formatCurrency(newTotal));
        
        // Update calculations display
        if (dimensions) {
            updateCalculationDisplay(dimensions, priceBreakdown);
        }
        
        // Update detailed breakdown
        updateDetailedBreakdown(priceBreakdown.items);
    }

    // Update individual price row
    function updatePriceRow(selector, value) {
        const $row = $(selector).closest('.price-row');
        const $value = $(selector);
        
        if (value && Math.abs(value) > 0.01) {
            $value.text(formatCurrency(value));
            $row.show();
        } else {
            $row.hide();
        }
    }

    // Update calculation display
    function updateCalculationDisplay(dimensions, priceBreakdown) {
        if (dimensions.area) {
            $('#area-calculation').text(`${dimensions.area.toFixed(4)} ${config.primaryUnit}`);
        }
        
        if (dimensions.perimeter) {
            $('#perimeter-calculation').text(`${dimensions.perimeter.toFixed(2)} m`);
        }
        
        if (dimensions.holes && dimensions.holes > 0) {
            $('#hole-calculation').text(`${dimensions.holes} holes`);
        }
    }

    // Update detailed price breakdown
    function updateDetailedBreakdown(items) {
        const $breakdown = $('#detailed-breakdown');
        $breakdown.empty();
        
        if (items && items.length > 0) {
            items.forEach(item => {
                if (item.affectsTotal && Math.abs(item.price) > 0.01) {
                    const $item = $(`
                        <div class="breakdown-item">
                            <div class="item-name">${item.name}</div>
                            <div class="item-description">${item.description}</div>
                            <div class="item-price">${formatCurrency(item.price)}</div>
                            ${item.formula ? `<div class="item-formula">Formula: ${item.formula}</div>` : ''}
                        </div>
                    `);
                    $breakdown.append($item);
                }
            });
            
            $('#price-details').show();
        } else {
            $('#price-details').hide();
        }
    }

    // Update configuration summary
    function updateConfigurationSummary() {
        const $summary = $('#selection-summary');
        $summary.empty();
        
        Object.keys(currentConfiguration).forEach(attributeName => {
            const value = currentConfiguration[attributeName];
            const $section = $(`.attribute-section[data-attribute="${attributeName}"]`);
            const displayName = $section.find('.attribute-title').text().replace('*', '').trim();
            
            let displayValue = value;
            
            // For single select, get display name
            if ($section.data('input-type') === 'SingleSelect') {
                const $selectedOption = $section.find(`.option-input[value="${value}"]`);
                if ($selectedOption.length) {
                    displayValue = $selectedOption.closest('.option-card').find('.option-name').text();
                }
            }
            
            $summary.append(`
                <div class="summary-item">
                    <span class="summary-label">${displayName}:</span>
                    <span class="summary-value">${displayValue}</span>
                </div>
            `);
        });
    }

    // Update option selection UI
    function updateOptionSelection($section) {
        $section.find('.option-card').removeClass('selected');
        $section.find('.option-input:checked').closest('.option-card').addClass('selected');
    }

    // Save configuration
    async function saveConfiguration() {
        if (Object.keys(currentConfiguration).length === 0) {
            showError('Please configure the product before saving');
            return;
        }

        try {
            showButtonLoading('#save-config-btn');
            
            const response = await $.ajax({
                url: config.endpoints.saveConfiguration,
                type: 'POST',
                contentType: 'application/json',
                data: JSON.stringify({
                    configurableProductId: config.configurableProductId,
                    configuration: currentConfiguration,
                    configurationName: `Configuration ${new Date().toLocaleString()}`
                })
            });

            if (response.success) {
                showSuccess(response.message || 'Configuration saved successfully');
            } else {
                showError(response.message || 'Failed to save configuration');
            }
        } catch (error) {
            showError('Failed to save configuration: ' + error.message);
        } finally {
            hideButtonLoading('#save-config-btn');
        }
    }

    // Generate quote
    async function generateQuote() {
        if (Object.keys(currentConfiguration).length === 0) {
            showError('Please configure the product before generating a quote');
            return;
        }

        try {
            showButtonLoading('#generate-quote-btn');
            
            const response = await $.ajax({
                url: config.endpoints.generateQuote,
                type: 'POST',
                contentType: 'application/json',
                data: JSON.stringify({
                    configurableProductId: config.configurableProductId,
                    configuration: currentConfiguration
                })
            });

            if (response.success) {
                displayQuote(response.quote);
                $('#quoteModal').modal('show');
            } else {
                showError(response.message || 'Failed to generate quote');
            }
        } catch (error) {
            showError('Failed to generate quote: ' + error.message);
        } finally {
            hideButtonLoading('#generate-quote-btn');
        }
    }

    // Display quote in modal
    function displayQuote(quote) {
        const $content = $('#quote-content');
        $content.html(`
            <div class="quote-container">
                <div class="quote-section">
                    <h5>Configuration</h5>
                    <div class="quote-configuration">
                        ${Object.keys(quote.configuration).map(key => 
                            `<div class="config-item">
                                <strong>${key}:</strong> ${quote.configuration[key]}
                             </div>`
                        ).join('')}
                    </div>
                </div>
                
                <div class="quote-section">
                    <h5>Dimensions</h5>
                    <div class="quote-dimensions">
                        ${Object.keys(quote.dimensions).map(key => 
                            `<div class="dimension-item">
                                <strong>${key}:</strong> ${quote.dimensions[key]} ${key === 'area' ? config.primaryUnit : 'm'}
                             </div>`
                        ).join('')}
                    </div>
                </div>
                
                <div class="quote-section">
                    <h5>Pricing</h5>
                    <div class="quote-pricing">
                        <div class="total-price">
                            <strong>Total Price: ${formatCurrency(quote.pricing.totalPrice)}</strong>
                        </div>
                        <div class="price-breakdown-details">
                            ${quote.pricing.breakdown.map(item => 
                                `<div class="breakdown-detail">
                                    ${item.name}: ${formatCurrency(item.price)}
                                 </div>`
                            ).join('')}
                        </div>
                    </div>
                </div>
                
                <div class="quote-section">
                    <h5>Production Information</h5>
                    <div class="quote-production">
                        <div><strong>Production Time:</strong> ${quote.production.totalProductionTime} minutes</div>
                        <div><strong>Production Cost:</strong> ${formatCurrency(quote.production.totalProductionCost)}</div>
                        <div><strong>Estimated Delivery:</strong> ${new Date(quote.production.estimatedDeliveryDate).toLocaleDateString()}</div>
                        <div><strong>Work Centers:</strong> ${quote.production.workCenters.join(', ')}</div>
                    </div>
                </div>
                
                <div class="quote-section">
                    <h5>Bill of Materials (Top Items)</h5>
                    <div class="quote-bom">
                        <table class="table table-sm">
                            <thead>
                                <tr>
                                    <th>Part Number</th>
                                    <th>Description</th>
                                    <th>Qty</th>
                                    <th>Cost</th>
                                </tr>
                            </thead>
                            <tbody>
                                ${quote.bom.items.map(item => 
                                    `<tr>
                                        <td>${item.partNumber}</td>
                                        <td>${item.description}</td>
                                        <td>${item.quantity} ${item.unit}</td>
                                        <td>${formatCurrency(item.totalCost)}</td>
                                     </tr>`
                                ).join('')}
                            </tbody>
                        </table>
                        <div class="bom-summary">
                            <div><strong>Total Material Cost:</strong> ${formatCurrency(quote.bom.totalMaterialCost)}</div>
                            <div><strong>Lead Time:</strong> ${quote.bom.leadTimeDays} days</div>
                        </div>
                    </div>
                </div>
            </div>
        `);
    }

    // Add to cart (placeholder)
    function addToCart() {
        if (Object.keys(currentConfiguration).length === 0) {
            showError('Please configure the product before adding to cart');
            return;
        }

        // This would integrate with nopCommerce cart system
        showInfo('Add to cart functionality would be implemented here');
    }

    // UI Helper Functions
    function showLoading() {
        $('.loading-indicator').show();
    }

    function hideLoading() {
        $('.loading-indicator').hide();
    }

    function showButtonLoading(selector) {
        const $btn = $(selector);
        $btn.prop('disabled', true);
        $btn.find('i').removeClass().addClass('fas fa-spinner fa-spin');
    }

    function hideButtonLoading(selector) {
        const $btn = $(selector);
        $btn.prop('disabled', false);
        $btn.find('i').removeClass('fa-spinner fa-spin');
        
        // Restore original icon based on button
        if (selector.includes('save')) {
            $btn.find('i').addClass('fas fa-save');
        } else if (selector.includes('quote')) {
            $btn.find('i').addClass('fas fa-file-invoice');
        } else if (selector.includes('cart')) {
            $btn.find('i').addClass('fas fa-shopping-cart');
        }
    }

    function showError(message) {
        // Could integrate with nopCommerce notification system
        console.error(message);
        alert('Error: ' + message);
    }

    function showSuccess(message) {
        // Could integrate with nopCommerce notification system
        console.log(message);
        alert('Success: ' + message);
    }

    function showInfo(message) {
        // Could integrate with nopCommerce notification system
        console.info(message);
        alert('Info: ' + message);
    }

    function formatCurrency(value) {
        return new Intl.NumberFormat('en-US', {
            style: 'currency',
            currency: 'USD'
        }).format(value);
    }

    // Public API
    return {
        init: init,
        updateConfiguration: updateConfiguration,
        setConfiguration: function(configuration) {
            currentConfiguration = configuration;
            updateFormFromConfiguration(configuration);
            updateConfiguration();
        },
        getConfiguration: function() {
            return currentConfiguration;
        }
    };

})();
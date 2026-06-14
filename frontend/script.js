// Available conversion categories and their units
const categories = {
  length: [
    "millimeter",
    "centimeter",
    "meter",
    "kilometer",
    "inch",
    "foot",
    "yard",
    "mile",
  ],

  mass: ["milligram", "gram", "kilogram", "ounce", "pound"],

  temperature: ["celsius", "fahrenheit", "kelvin"],
};

const categorySelect = document.getElementById("category");
const fromUnitSelect = document.getElementById("fromUnit");
const toUnitSelect = document.getElementById("toUnit");

const valueInput = document.getElementById("value");
const convertButton = document.getElementById("convertButton");
const resultElement = document.getElementById("result");
const historyElement = document.getElementById("history");

// Copy button element
const copyResultButton = document.getElementById("copyResultButton");

// Debounce timer for auto-convert
let debounceTimer = null;

// Cache: avoids duplicate API calls
const cache = new Map();

// History (max 20 items)
const history = [];

// Stores last numeric result for copy feature
let lastResultValue = null;

// Fill the category dropdown from the categories map
function populateCategories() {
  categorySelect.innerHTML = "";

  for (const categoryName in categories) {
    const option = document.createElement("option");

    option.value = categoryName;
    option.textContent = categoryName;

    categorySelect.append(option);
  }
}

// Populate the source unit dropdown for the selected category
function populateFromUnits(categoryName) {
  fromUnitSelect.innerHTML = "";

  const units = categories[categoryName];

  for (const unit of units) {
    const option = document.createElement("option");
    option.value = unit;
    option.textContent = unit;

    fromUnitSelect.append(option);
  }
}

// Populate the target unit dropdown, excluding the selected source unit
function populateToUnits() {
  toUnitSelect.innerHTML = "";

  const units = categories[categorySelect.value];
  const selectedFrom = fromUnitSelect.value;

  for (const unit of units) {
    if (unit === selectedFrom) continue;

    const option = document.createElement("option");
    option.value = unit;
    option.textContent = unit;

    toUnitSelect.append(option);
  }

  // Reset value when units change
  valueInput.value = 0;
}

// Build cache key
function buildCacheKey(request) {
  return `${request.category}|${request.fromUnit}|${request.toUnit}|${request.value}`;
}

// Add item to history
function addToHistory(request, result) {
  const text = `${request.value} ${request.fromUnit} to ${result.result} ${result.toUnit}`;

  history.unshift(text);

  if (history.length > 20) {
    history.pop();
  }

  renderHistory();
}

// Render history list
function renderHistory() {
  historyElement.innerHTML = "";

  for (const item of history) {
    const li = document.createElement("li");
    li.textContent = item;
    historyElement.appendChild(li);
  }
}

// Send request to backend and render result
async function convert() {
  const numericValue = Number(valueInput.value);

  if (numericValue === 0) {
    resultElement.textContent = "0";
    return;
  }

  if (Number.isNaN(numericValue)) {
    resultElement.textContent = "Invalid number";
    return;
  }

  const request = {
    category: categorySelect.value,
    fromUnit: fromUnitSelect.value,
    toUnit: toUnitSelect.value,
    value: numericValue,
  };

  const cacheKey = buildCacheKey(request);

  // CACHE HIT → no API call
  if (cache.has(cacheKey)) {
    const cached = cache.get(cacheKey);

    lastResultValue = cached.result;
    resultElement.textContent = `${cached.result} ${cached.toUnit}`;

    addToHistory(request, cached);
    return;
  }

  console.log("Request:", request);

  const response = await fetch("http://localhost:5117/convert", {
    method: "POST",
    headers: {
      "Content-Type": "application/json",
    },
    body: JSON.stringify(request),
  });

  console.log("Status:", response.status);

  const data = await response.json();

  console.log("Response:", data);

  // save cache
  cache.set(cacheKey, data);

  lastResultValue = data.result;
  resultElement.textContent = `${data.result} ${data.toUnit}`;

  addToHistory(request, data);
}

// Debounce wrapper for auto conversion
function triggerAutoConvert() {
  clearTimeout(debounceTimer);

  debounceTimer = setTimeout(() => {
    convert();
  }, 400);
}

// Swap units feature (⇄)
function swapUnits() {
  const temp = fromUnitSelect.value;
  fromUnitSelect.value = toUnitSelect.value;
  toUnitSelect.value = temp;

  populateToUnits();

  // Trigger recalculation after swap
  triggerAutoConvert();
}

// Set up the initial dropdown values when the page loads
function init() {
  populateCategories();
  populateFromUnits(categorySelect.value);
  populateToUnits();
}

init();

// Refresh unit options when the category changes
categorySelect.addEventListener("change", () => {
    populateFromUnits(categorySelect.value);
    populateToUnits();
    valueInput.value = 0; // reset value
    triggerAutoConvert();
});

// Keep the target units in sync when the source unit changes
fromUnitSelect.addEventListener("change", () => {
  populateToUnits();
  valueInput.value = 0; // reset value
  triggerAutoConvert();
});

// Auto convert on input
valueInput.addEventListener("input", () => {
  triggerAutoConvert();
});

// Manual convert button (fallback)
convertButton.addEventListener("click", () => {
  convert();
});

// Swap via keyboard shortcut
document.addEventListener("keydown", (e) => {
  if (e.ctrlKey && e.key === "r") {
    swapUnits();
  }
});

// Copy result (ONLY numeric value)
copyResultButton.addEventListener("click", async () => {
  if (lastResultValue === null) return;

  await navigator.clipboard.writeText(lastResultValue.toString());
});
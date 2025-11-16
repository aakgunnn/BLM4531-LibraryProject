// API Client Service
const API_BASE_URL = window.location.origin;

const api = {
  // Helper method for making requests
  async request(endpoint, options = {}) {
    const token = localStorage.getItem("token");

    const config = {
      headers: {
        "Content-Type": "application/json",
        ...(token && { Authorization: `Bearer ${token}` }),
      },
      ...options,
    };

    if (options.body) {
      config.body = JSON.stringify(options.body);
    }

    try {
      const response = await fetch(`${API_BASE_URL}${endpoint}`, config);

      // Handle unauthorized
      if (response.status === 401) {
        localStorage.removeItem("token");
        localStorage.removeItem("user");
        window.location.href = "/pages/login.html";
        throw new Error("Oturum süreniz dolmuş. Lütfen tekrar giriş yapın.");
      }

      // Handle No Content (204) or empty responses
      if (
        response.status === 204 ||
        response.headers.get("content-length") === "0"
      ) {
        if (!response.ok) {
          throw new Error("Bir hata oluştu");
        }
        return { success: true };
      }

      // Try to parse JSON, handle empty responses
      let data;
      const contentType = response.headers.get("content-type");
      if (contentType && contentType.includes("application/json")) {
        const text = await response.text();
        data = text ? JSON.parse(text) : { success: true };
      } else {
        data = { success: true };
      }

      if (!response.ok) {
        throw new Error(data.message || data.title || "Bir hata oluştu");
      }

      return data;
    } catch (error) {
      if (error.name === "TypeError" && error.message === "Failed to fetch") {
        throw new Error(
          "Sunucuya bağlanılamadı. Lütfen internet bağlantınızı kontrol edin."
        );
      }
      if (error.name === "SyntaxError") {
        throw new Error("Sunucudan geçersiz yanıt alındı.");
      }
      throw error;
    }
  },

  // HTTP Methods
  get(endpoint) {
    return this.request(endpoint, { method: "GET" });
  },

  post(endpoint, body) {
    return this.request(endpoint, { method: "POST", body });
  },

  put(endpoint, body) {
    return this.request(endpoint, { method: "PUT", body });
  },

  delete(endpoint) {
    return this.request(endpoint, { method: "DELETE" });
  },
};

// Helper functions
function showLoading() {
  const loadingHtml = `
        <div class="spinner-overlay" id="loadingSpinner">
            <div class="spinner-border text-light" style="width: 3rem; height: 3rem;" role="status">
                <span class="visually-hidden">Yükleniyor...</span>
            </div>
        </div>
    `;
  document.body.insertAdjacentHTML("beforeend", loadingHtml);
}

function hideLoading() {
  const spinner = document.getElementById("loadingSpinner");
  if (spinner) {
    spinner.remove();
  }
}

function formatDate(dateString) {
  const date = new Date(dateString);
  return date.toLocaleDateString("tr-TR", {
    year: "numeric",
    month: "long",
    day: "numeric",
  });
}

function formatDateTime(dateString) {
  const date = new Date(dateString);
  return date.toLocaleString("tr-TR", {
    year: "numeric",
    month: "long",
    day: "numeric",
    hour: "2-digit",
    minute: "2-digit",
  });
}

function getStatusBadge(status) {
  const badges = {
    Pending: '<span class="badge badge-pending">Beklemede</span>',
    Borrowed: '<span class="badge badge-borrowed">Ödünç Alındı</span>',
    Returned: '<span class="badge badge-returned">İade Edildi</span>',
    Late: '<span class="badge badge-late">Gecikmiş</span>',
    Cancelled: '<span class="badge bg-secondary">İptal Edildi</span>',
  };
  return badges[status] || status;
}

function getAvailabilityBadge(isAvailable) {
  return isAvailable
    ? '<span class="badge badge-available">Müsait</span>'
    : '<span class="badge badge-unavailable">Ödünçte</span>';
}

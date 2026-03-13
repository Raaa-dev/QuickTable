import { defineStore } from "pinia";
import { ref } from "vue";
import axios from "axios";

export const useTableStore = defineStore("table", () => {
  const token = ref<string>(localStorage.getItem("tableToken") || "");
  const tableNumber = ref<string>(localStorage.getItem("tableNumber") || "");
  const sessionId = ref<number | null>(  // ✅ DECLARE IT HERE
    localStorage.getItem("sessionId") ? Number(localStorage.getItem("sessionId")) : null
  );

  const setToken = async (qrToken: string) => {
    token.value = qrToken;
    localStorage.setItem("tableToken", qrToken);

    try {
      const res = await axios.get(`/api/v1/Table/Resolve?token=${qrToken}`);
      console.log("Table resolve response:", res.data);

      // Check if new session is different from old session
      const newSessionId = res.data?.sessionId;
      const oldSessionId = localStorage.getItem("sessionId");

      // ✅ Different session = new customer = clear old history
      if (oldSessionId && String(newSessionId) !== oldSessionId) {
        localStorage.removeItem("orderHistory");
        localStorage.removeItem("cart");
        console.log("New session detected! Cleared old history.");
      }

      tableNumber.value = res.data?.table || "";
      sessionId.value = newSessionId || null;

      localStorage.setItem("tableNumber", tableNumber.value);
      if (sessionId.value) {
        localStorage.setItem("sessionId", String(sessionId.value));
      }
    } catch (err) {
      console.error("Invalid token", err);
    }
  };

  const checkAndClearIfClosed = async () => {
    const storedSessionId = localStorage.getItem("sessionId");
    if (!storedSessionId) return;  // no session = nothing to check

    try {
      const res = await axios.get(`/api/v1/Table/session/${storedSessionId}`);
      if (res.data?.status === "Closed") {
        console.log("Session closed by staff, clearing data...");
        clear();
        localStorage.removeItem("orderHistory");
        localStorage.removeItem("cart");
      }
    } catch (err) {
      console.warn("Could not check session status");
    }
  };

  const clear = () => {
    token.value = "";
    tableNumber.value = "";
    sessionId.value = null;
    localStorage.removeItem("tableToken");
    localStorage.removeItem("tableNumber");
    localStorage.removeItem("sessionId"); // ✅ remove sessionId too!
  };

  return { token, tableNumber, sessionId, setToken, clear, checkAndClearIfClosed };
});
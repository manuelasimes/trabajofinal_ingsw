import { defineConfig } from "cypress";

export default defineConfig({
  e2e: {
    baseUrl: "http://localhost:4200", // Agrega esto
    setupNodeEvents(on, config) {
      // Implementa eventos aquí si es necesario
    },
  },
});

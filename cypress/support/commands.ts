/// <reference types="cypress" />

declare global {
    namespace Cypress {
      interface Chainable {
        login(): Chainable<void>;
      }
    }
  }
  
  Cypress.Commands.add('login', () => {
    cy.intercept('POST', '**/api/auth/login', { message: 'fake-jwt-token' }).as('loginRequest');
    cy.visit('/login');
    cy.get('input[name="username"]').type('usuario');
    cy.get('input[name="password"]').type('contraseña');
    cy.get('button').click();
    cy.wait('@loginRequest');
  });
  
  export {}; // Esto es necesario para evitar conflictos con el alcance global de TypeScript.
  
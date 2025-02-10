describe('Autenticación', () => {
    beforeEach(() => {
        //cy.visit('/login')
      cy.visit('http://localhost:4200/login'); // Ir a la pantalla de login antes de cada prueba
    });
  
    it('Debe iniciar sesión con credenciales correctas', () => {
      cy.intercept('POST', '**/api/auth/login', { message: 'fake-jwt-token' }).as('loginRequest');
  
      cy.get('input[name="username"]').type('usuario');
      cy.get('input[name="password"]').type('contraseña');
      cy.get('button').click();
  
      cy.wait('@loginRequest');
      cy.url().should('include', '/articles');
    });
  
    it('Debe mostrar un error con credenciales incorrectas', () => {
      cy.intercept('POST', '**/api/auth/login', { statusCode: 401 }).as('loginFailed');
  
      cy.get('input[name="username"]').type('usuario_invalido');
      cy.get('input[name="password"]').type('contraseña_mala');
      cy.get('button').click();
  
      cy.wait('@loginFailed');
      cy.on('window:alert', (text) => {
        expect(text).to.contains('Login failed');
      });
    });
  });
  
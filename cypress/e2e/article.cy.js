describe('Gestión de Artículos', () => {
    beforeEach(() => {
      cy.login(); // 🔹 Función que crearemos en Cypress para loguearnos automáticamente
      cy.visit('/articles');
    });
  
    it('Debe mostrar la lista de artículos', () => {
      cy.intercept('GET', '**/api/article', { fixture: 'articles.json' }).as('getArticles');
      cy.wait('@getArticles');
  
      cy.get('.articles-container').should('have.length.greaterThan', 0);
    });
  
    it('Debe agregar un nuevo artículo', () => {
      cy.get('input[name="title"]').type('Nuevo artículo');
      cy.get('textarea[name="description"]').type('Descripción de prueba');
      cy.get('button').contains('Agregar').click();
  
      cy.get('.articles-container').contains('Nuevo artículo');
    });
  });
  
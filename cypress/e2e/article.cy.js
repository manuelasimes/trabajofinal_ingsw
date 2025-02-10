describe('Gestión de Artículos', () => {
  beforeEach(() => {
    cy.login(); // Verifica que esta función hace el login correctamente
    cy.url().should('include', '/articles'); // Verifica que la URL es la correcta
    cy.visit('/articles');
  });

  it('Debe mostrar la lista de artículos', () => {
    // Interceptamos la solicitud GET para /api/article
    cy.intercept('GET', 'http://localhost:5266/api/article', { fixture: 'articles.json' }).as('getArticles');


    // Verificamos que los artículos sean mostrados
    cy.get('.articles-container').should('have.length.greaterThan', 0);
  });

  it('Debe agregar un nuevo artículo', () => {
    cy.get('input[name="title"]').type('Nuevo artículo');
    cy.get('textarea[name="description"]').type('Descripción de prueba');
    cy.get('button').contains('Agregar').click();

    // Verificamos que el nuevo artículo haya sido agregado
    cy.get('.articles-container').contains('Nuevo artículo');
  });
});

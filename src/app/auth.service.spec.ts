import { TestBed } from '@angular/core/testing';
import { HttpTestingController, HttpClientTestingModule } from '@angular/common/http/testing';  // Importar HttpClientTestingModule
import { AuthService } from './auth.service';
import { AppComponent } from './app.component';
import { LoginComponent } from './login/login.component';
import { ArticleService } from './article.service';

describe('AuthService', () => {
  let service: AuthService;
  let httpMock: HttpTestingController;

  const mockToken = 'mock-jwt-token';
  const mockUsername = 'user';
  const mockPassword = 'password';

  beforeEach(() => {
    TestBed.configureTestingModule({
      imports: [HttpClientTestingModule], // Asegúrate de agregar HttpClientTestingModule
      providers: [AuthService, ArticleService] // No es necesario proporcionar 'provideHttpClientTesting' si ya usas HttpClientTestingModule
    });
    service = TestBed.inject(AuthService);
    httpMock = TestBed.inject(HttpTestingController);
  });
  it('should render title', () => {
    const fixture = TestBed.createComponent(AppComponent);
    const app = fixture.componentInstance;
    fixture.detectChanges(); // Asegura que los cambios se apliquen en el template
    const compiled = fixture.nativeElement as HTMLElement;
    expect(compiled.querySelector('h1')?.textContent).toContain('Hello, article-app');  // Cambié la expectativa
  });
   
  

  it('should be created', () => {
    expect(service).toBeTruthy();
  });

  it('should login and save token', () => {
    const loginResponse = { token: mockToken };

    // Realizamos la llamada al servicio de login
    service.login(mockUsername, mockPassword).subscribe(response => {
      // Verificamos que el token devuelto sea el esperado
      expect(response.token).toBe(mockToken);
      // Verificamos que el token se haya guardado en localStorage
      expect(localStorage.getItem('jwt')).toBe(mockToken);
    });

    // Simulamos la solicitud HTTP de login
    const req = httpMock.expectOne('http://localhost:5266/api/auth/login');
    expect(req.request.method).toBe('POST'); // Verificamos que el método sea POST
    req.flush(loginResponse); // Proporcionamos la respuesta simulada

    httpMock.verify(); // Verificamos que no haya solicitudes pendientes
  });

  it('should return true if user is authenticated', () => {
    localStorage.setItem('jwt', mockToken); // Simulamos que el token está en localStorage
    expect(service.isAuthenticated()).toBeTrue(); // Verificamos que el usuario esté autenticado
  });

  it('should return false if user is not authenticated', () => {
    localStorage.removeItem('jwt'); // Simulamos que no hay token en localStorage
    expect(service.isAuthenticated()).toBeFalse(); // Verificamos que el usuario no esté autenticado
  });

  it('should logout and remove token', () => {
    localStorage.setItem('jwt', mockToken); // Simulamos que el token está en localStorage

    service.logout(); // Ejecutamos el logout

    expect(localStorage.getItem('jwt')).toBeNull(); // Verificamos que el token haya sido removido
  });

  afterEach(() => {
    httpMock.verify(); // Verificamos que no haya solicitudes pendientes
  });
});

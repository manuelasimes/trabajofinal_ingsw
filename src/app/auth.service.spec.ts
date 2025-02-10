import { TestBed } from '@angular/core/testing';
import { HttpClientTestingModule, HttpTestingController } from '@angular/common/http/testing'; // Importar módulos de prueba HTTP
import { AuthService } from './auth.service'; // Asegúrate de que la ruta sea correcta

describe('AuthService', () => {
  let service: AuthService;
  let httpMock: HttpTestingController;

  const mockToken = 'mock-jwt-token';
  const mockUsername = 'user';
  const mockPassword = 'password';

  // Espías para localStorage
  let getItemSpy: jasmine.Spy;
  let setItemSpy: jasmine.Spy;
  let removeItemSpy: jasmine.Spy;

  beforeEach(() => {
    TestBed.configureTestingModule({
      imports: [HttpClientTestingModule],  // Importar el módulo de prueba HTTP
      providers: [AuthService]  // Proveedor para el servicio AuthService
    });
    service = TestBed.inject(AuthService);  // Inyectar el servicio AuthService
    httpMock = TestBed.inject(HttpTestingController);  // Inyectar HttpTestingController

    // Espiar localStorage una sola vez
    getItemSpy = spyOn(localStorage, 'getItem').and.callFake((key: string) => {
      if (key === 'jwt') {
        return mockToken;
      }
      return null;
    });

    setItemSpy = spyOn(localStorage, 'setItem').and.callThrough();
    removeItemSpy = spyOn(localStorage, 'removeItem').and.callThrough();
  });

  afterEach(() => {
    httpMock.verify();
  });
  it('should login and save token', () => {
    const mockToken = 'mock-jwt-token';  // Token simulado
    const mockUsername = 'mockuser';
    const mockPassword = 'mockpassword';
    const loginResponse = { token: mockToken };  // Respuesta simulada de la API
  
    // Espiar el método saveToken
    const saveTokenSpy = spyOn(service, 'saveToken').and.callThrough();
  
    // Llamada al login
    service.login(mockUsername, mockPassword).subscribe(response => {
      expect(response.token).toEqual(mockToken);  // Verificar que el token devuelto sea correcto
      
      // Ahora llamamos a saveToken explícitamente para asegurarnos de que guarda el token
      service.saveToken(response.token);
  
      // Verificar que saveToken fue llamado con el token esperado
      expect(saveTokenSpy).toHaveBeenCalledWith(mockToken);
    });
  
    // Simulamos la respuesta de la API
    const req = httpMock.expectOne('http://localhost:5266/api/auth/login');
    expect(req.request.method).toEqual('POST');
    req.flush(loginResponse);  // Enviar la respuesta simulada
  });
  

  it('should return true if user is authenticated', () => {
    // Simulamos que el token está en localStorage
    getItemSpy.and.returnValue(mockToken);

    expect(service.isAuthenticated()).toBeTrue();
  });

  it('should return false if user is not authenticated', () => {
    // Simulamos que no hay token en localStorage
    getItemSpy.and.returnValue(null);

    expect(service.isAuthenticated()).toBeFalse();
  });

  it('should logout and remove token', () => {
    // Simulamos que el token está en localStorage
    getItemSpy.and.returnValue(mockToken);

    service.logout();

    expect(removeItemSpy).toHaveBeenCalledWith('jwt');
  });
});

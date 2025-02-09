import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { Observable } from 'rxjs';
import { HttpHeaders } from '@angular/common/http';

@Injectable({
  providedIn: 'root'
})
export class AuthService {

  private apiUrl = 'http://localhost:5266/api/auth/login';  // URL de login

  constructor(private http: HttpClient) { }

  login(username: string, password: string): Observable<any> {
    const headers = new HttpHeaders({ 'Content-Type': 'application/json' });
    return this.http.post<any>(this.apiUrl, { username, password }, { headers });
  }

  // Guardar token en localStorage
  saveToken(token: string): void {
    localStorage.setItem('jwt', token);
  }

  // Obtener token desde localStorage
  getToken(): string | null {
    return localStorage.getItem('jwt');
  }

  // Verificar si el usuario está autenticado
  isAuthenticated(): boolean {
    return this.getToken() !== null;
  }

  // Eliminar el token al hacer logout
  logout(): void {
    localStorage.removeItem('jwt');
  }
}

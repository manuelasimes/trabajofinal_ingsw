import { Injectable } from '@angular/core';
import { HttpClient, HttpHeaders } from '@angular/common/http';
import { Observable } from 'rxjs';
import { Article } from './article';
import { AuthService } from './auth.service';

@Injectable({
  providedIn: 'root'
})
export class ArticleService {

  private apiUrl = 'http://localhost:5266/api/article';
  private uploadUrl = 'http://localhost:5266/api/article/upload';  // Aquí va la URL de tu API para subir imágenes

  constructor(private http: HttpClient, private authService: AuthService) { }

  // Obtener los headers con el token de autenticación
  private getHeaders(): HttpHeaders {
    const token = this.authService.getToken();
    return new HttpHeaders({
      'Authorization': `Bearer ${token}`
    });
  }
  // Subir imagen
  uploadImage(file: File): Observable<string> {
    const formData = new FormData();
    formData.append('file', file, file.name);
    
    return this.http.post<string>(this.uploadUrl, formData, { headers: this.getHeaders() });
  }
  // Obtener artículos
  getArticles(): Observable<Article[]> {
    return this.http.get<Article[]>(this.apiUrl, { headers: this.getHeaders() });
  }

  // Agregar un nuevo artículo
  addArticle(article: Article): Observable<Article> {
    return this.http.post<Article>(this.apiUrl, article, { headers: this.getHeaders() });
  }

  // Eliminar un artículo
  deleteArticle(id: number): Observable<void> {
    return this.http.delete<void>(`${this.apiUrl}/${id}`, { headers: this.getHeaders() });
  }
}

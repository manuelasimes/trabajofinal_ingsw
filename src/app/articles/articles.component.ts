import { Component, OnInit } from '@angular/core';
import { ArticleService } from '../article.service';
import { Article } from '../article';
import { AuthService } from '../auth.service';
import { Router } from '@angular/router';
import { CommonModule } from '@angular/common';
import { FormsModule } from '@angular/forms'; // ✅ Importar FormsModule

@Component({
  selector: 'app-articles',
  templateUrl: './articles.component.html',
  styleUrls: ['./articles.component.css'],
  imports: [CommonModule, FormsModule]  // 🔹 Agregar aquí también
})
export class ArticlesComponent implements OnInit {
  articles: Article[] = [];
  newArticle: Article = {
    title: '',
    description: '',
    imageUrl: '',  // Este campo lo vamos a actualizar con el nombre de la imagen después de la carga
    date: new Date(),
    id: 0
  };

  selectedFile: File | null = null;  // Para almacenar el archivo seleccionado

  constructor(
    private articleService: ArticleService,
    private authService: AuthService,
    private router: Router
  ) { }

  ngOnInit(): void {
    // Verificar si está autenticado
    if (!this.authService.isAuthenticated()) {
      this.router.navigate(['/login']);
    } else {
      this.articleService.getArticles().subscribe(data => {
        this.articles = data;
      });
    }
  }

  onFileSelect(event: any): void {
    const file = event.target.files[0];
    if (file) {
      this.selectedFile = file;
    }
  }

  addArticle(): void {
    if (this.selectedFile) {
      this.articleService.uploadImage(this.selectedFile).subscribe(imageUrl => {
        this.newArticle.imageUrl = imageUrl;  // Guardamos el URL de la imagen recibida del servidor
        this.articleService.addArticle(this.newArticle).subscribe(article => {
          this.articles.push(article);
        });
      });
    } else {
      this.articleService.addArticle(this.newArticle).subscribe(article => {
        this.articles.push(article);
      });
    }
  }

  deleteArticle(id: number): void {
    this.articleService.deleteArticle(id).subscribe(() => {
      this.articles = this.articles.filter(article => article.id !== id);
    });
  }

  logout(): void {
    this.authService.logout();
    this.router.navigate(['/login']);
  }
}

import { Component } from '@angular/core';
import { AuthService } from '../auth.service';
import { Router } from '@angular/router';
import { CommonModule } from '@angular/common';
import { FormsModule } from '@angular/forms'; // ✅ Importar FormsModule
@Component({
  selector: 'app-login',
  templateUrl: './login.component.html',
  styleUrls: ['./login.component.css'],
  imports: [CommonModule, FormsModule]  // 🔹 Agregar aquí también
  
})
export class LoginComponent {

  username: string = '';
  password: string = '';

  constructor(private authService: AuthService, private router: Router) { }

  // Método para hacer login
  login(): void {
    this.authService.login(this.username, this.password).subscribe(response => {
      // Guardar el token en el localStorage
      this.authService.saveToken(response.message);
      // Redirigir a la página principal
      this.router.navigate(['/articles']);
    }, error => {
      alert('Login failed');
    });
  }
}

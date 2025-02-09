import { bootstrapApplication } from '@angular/platform-browser';
import { AppComponent } from './app/app.component';
import { importProvidersFrom } from '@angular/core';
import { BrowserModule } from '@angular/platform-browser';
import { FormsModule } from '@angular/forms';
import { CommonModule } from '@angular/common';
import { AppRoutingModule } from './app/app.routes'; // Asegúrate de importar el routing module
import { HttpClientModule } from '@angular/common/http'; // 🔹 Importar HttpClientModule

bootstrapApplication(AppComponent, {
  providers: [
    importProvidersFrom(BrowserModule, FormsModule, CommonModule, AppRoutingModule, HttpClientModule) // Agrega el AppRoutingModule
  ]
}).catch(err => console.error(err));

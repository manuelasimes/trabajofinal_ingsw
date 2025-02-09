export interface Article {
  id: number;  
  title: string;
  description: string;
  imageUrl: string;
  date: string | Date; // ✅ Soporta ambos formatos
}

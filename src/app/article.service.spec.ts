  import { TestBed } from '@angular/core/testing';
  import { HttpClientTestingModule, HttpTestingController } from '@angular/common/http/testing';
  import { ArticleService } from './article.service';
  import { AuthService } from './auth.service';
  import { Article } from './article';
  import { ArticlesComponent } from './articles/articles.component';

  describe('ArticleService', () => {
    let service: ArticleService;
    let httpMock: HttpTestingController;
    let authServiceSpy: jasmine.SpyObj<AuthService>;

    beforeEach(() => {
      const authSpy = jasmine.createSpyObj('AuthService', ['getToken']);

      TestBed.configureTestingModule({
        imports: [HttpClientTestingModule],
        providers: [
          ArticleService,
          { provide: AuthService, useValue: authSpy }
        ]
      });

      service = TestBed.inject(ArticleService);
      httpMock = TestBed.inject(HttpTestingController);
      authServiceSpy = TestBed.inject(AuthService) as jasmine.SpyObj<AuthService>;

      authServiceSpy.getToken.and.returnValue('mock-token');
    });

    afterEach(() => {
      httpMock.verify();
    });

    it('should be created', () => {
      expect(service).toBeTruthy();
    });

    it('should fetch articles', () => {
      const mockArticles: Article[] = [
        {
          id: 1, title: 'Test Article', description: 'Test Content',
          imageUrl: '',
          date: ''
        }
      ];

      service.getArticles().subscribe(articles => {
        expect(articles.length).toBe(1);
        expect(articles[0].title).toBe('Test Article');
      });

      const req = httpMock.expectOne('http://localhost:5266/api/article');
      expect(req.request.method).toBe('GET');
      expect(req.request.headers.get('Authorization')).toBe('Bearer mock-token');
      req.flush(mockArticles);
    });

    it('should add an article', () => {
      const newArticle: Article = {
        id: 2, title: 'New Article', description: 'New Content',
        imageUrl: '',
        date: ''
      };

      service.addArticle(newArticle).subscribe(article => {
        expect(article).toEqual(newArticle);
      });

      const req = httpMock.expectOne('http://localhost:5266/api/article');
      expect(req.request.method).toBe('POST');
      expect(req.request.headers.get('Authorization')).toBe('Bearer mock-token');
      req.flush(newArticle);
    });

    it('should delete an article', () => {
      const articleId = 1;
    
      service.deleteArticle(articleId).subscribe(response => {
        expect(response).toBeNull(); // Cambiado de toBeUndefined() a toBeNull()
      });
    
      const req = httpMock.expectOne(`http://localhost:5266/api/article/${articleId}`);
      expect(req.request.method).toBe('DELETE');
      expect(req.request.headers.get('Authorization')).toBe('Bearer mock-token');
      req.flush(null); // El backend típicamente devuelve null para DELETE
    });
    
  });

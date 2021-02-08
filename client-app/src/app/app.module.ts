import { BrowserModule } from '@angular/platform-browser';
import { NgModule } from '@angular/core';
import { BrowserAnimationsModule } from '@angular/platform-browser/animations';
import { FormsModule } from '@angular/forms';
import { HttpClientModule } from '@angular/common/http';
// routing
import { AppRoutingModule } from './app-routing.module';
// components
import { AppComponent } from './app.component';
import { LoginDialogComponent } from './components/login-dialog/login-dialog.component';
import { HeaderComponent } from './components/header/header.component';
// compack
import { CompackBannerModule, CompackDatepickerModule, CompackToastModule } from 'ngx-compack';
// mat
import { MatDialogModule } from '@angular/material/dialog';
import { MatFormFieldModule } from '@angular/material/form-field';
import { MatInputModule } from '@angular/material/input';
import { MatTooltipModule } from '@angular/material/tooltip';
// page
import { NotFoundPageComponent } from './pages/not-found-page/not-found-page.component';
// pipe
import { PostDateParsPipe } from './pipe/post-date-pars.pipe';
// interceptors
import { httpInterceptorProviders } from './interceptors/http-Interceptors';


@NgModule({
  declarations: [
    HeaderComponent,
    PostDateParsPipe,
    NotFoundPageComponent,
    AppComponent,
    LoginDialogComponent
  ],
  imports: [
    CompackDatepickerModule,
    MatTooltipModule,
    MatInputModule,
    MatDialogModule,
    MatFormFieldModule,
    HttpClientModule,
    CompackToastModule,
    CompackBannerModule,
    FormsModule,
    BrowserAnimationsModule,
    BrowserModule,
    AppRoutingModule
  ],
  providers: [httpInterceptorProviders],
  bootstrap: [AppComponent]
})
export class AppModule { }

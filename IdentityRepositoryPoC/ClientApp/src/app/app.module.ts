import { BrowserModule } from '@angular/platform-browser';
import { NgModule } from '@angular/core';
import { FormsModule, ReactiveFormsModule } from '@angular/forms';
import { HttpClientModule, HTTP_INTERCEPTORS } from '@angular/common/http';
import { RouterModule } from '@angular/router';
import { ToastrModule } from 'ngx-toastr';

import { AppComponent } from './app.component';
import { NavMenuComponent } from './nav-menu/nav-menu.component';
import { HomeComponent } from './home/home.component';
import { CounterComponent } from './counter/counter.component';
import { FetchDataComponent } from './fetch-data/fetch-data.component';
import { RegisterComponent } from './user/register/register.component';
import { BrowserAnimationsModule } from '@angular/platform-browser/animations';
import { UserComponent } from './user/user.component';
import { LoginComponent } from './user/login/login.component';
import { UserService } from './shared/user.service';
import { AuthInterceptor } from './auth/auth.interceptor';
import { AuthGuard } from './auth/auth.guard';
import { UserListComponent } from './user/user-list/user-list.component';
import { ForbiddenComponent } from './auth/forbidden/forbidden.component';
import { UnauthorizedComponent } from './auth/unauthorized/unauthorized.component';

@NgModule({
  declarations: [
    AppComponent,
    NavMenuComponent,
    HomeComponent,
    CounterComponent,
    FetchDataComponent,
    RegisterComponent,
    UserComponent,
    LoginComponent,
    UserListComponent,
    ForbiddenComponent,
    UnauthorizedComponent,
  ],
  imports: [
    BrowserModule.withServerTransition({ appId: 'ng-cli-universal' }),
    HttpClientModule,
    FormsModule,
    ReactiveFormsModule,
    BrowserAnimationsModule,
    ToastrModule.forRoot({
      progressBar: true
    }),
    RouterModule.forRoot([
      { path: '', redirectTo: 'user/login', pathMatch: 'full' },
      {
        path: 'user', component: UserComponent,
        children: [
          { path: 'registration', component: RegisterComponent },
          { path: 'login', component: LoginComponent }
        ]
      },
      { path: 'home', component: HomeComponent, canActivate: [AuthGuard] },
      { path: 'user/all', component: UserListComponent, canActivate: [AuthGuard] },
      { path: 'error/401', component: UnauthorizedComponent },
      { path: 'error/403', component: ForbiddenComponent },
    ])
  ],
  providers: [UserService, {
    provide: HTTP_INTERCEPTORS,
    useClass: AuthInterceptor,
    multi: true
  }],
  bootstrap: [AppComponent]
})
export class AppModule { }

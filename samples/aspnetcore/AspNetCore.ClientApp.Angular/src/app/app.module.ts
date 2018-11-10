import { BrowserModule } from '@angular/platform-browser';
import { NgModule } from '@angular/core';
import { RouterModule, Routes } from '@angular/router';
import { PrimaryComponent } from './primary.component';
import { SecondaryComponent } from './secondary.component';

import { AppComponent } from './app.component';

const appRoutes: Routes = [
  { path: 'primary', component: PrimaryComponent },
  { path: 'secondary', component: SecondaryComponent },
  {
    path: '',
    redirectTo: '/primary',
    pathMatch: 'full'
  }
];

@NgModule({
  declarations: [
    AppComponent,
    PrimaryComponent,
    SecondaryComponent
  ],
  imports: [
    BrowserModule,
    RouterModule.forRoot(appRoutes, {enableTracing: true})
  ],
  providers: [],
  bootstrap: [AppComponent]
})
export class AppModule { }

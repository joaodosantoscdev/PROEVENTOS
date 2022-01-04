import { BrowserModule } from '@angular/platform-browser';
import { NgModule } from '@angular/core';
import { HttpClientModule } from '@angular/common/http';
import { FormsModule } from '@angular/forms';
import { BrowserAnimationsModule } from '@angular/platform-browser/animations';
import { CollapseModule } from 'ngx-bootstrap/collapse';
import { AppRoutingModule } from './app-routing.module';

import { AppComponent } from './app.component';
import { EventsComponent } from './events/events.component';
import { SpeakersComponent } from './speakers/speakers.component';
import { NavComponent } from './nav/nav.component';


import { EventService } from './services/event.service';

import { DateFormatPipe } from './helpers/DateFormat.pipe';


@NgModule({
  declarations: [
    AppComponent,
    EventsComponent,
    SpeakersComponent,
    NavComponent,
    DateFormatPipe,
  ],
  imports: [
    BrowserModule,
    AppRoutingModule,
    HttpClientModule,
    BrowserAnimationsModule,
    CollapseModule.forRoot(),
    FormsModule
  ],
  providers: [
    EventService
  ],
  bootstrap: [AppComponent]
})
export class AppModule { }

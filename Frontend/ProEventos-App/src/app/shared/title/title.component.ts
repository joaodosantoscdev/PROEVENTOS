import { Component, Input, OnInit } from '@angular/core';
import { Router } from '@angular/router';

@Component({
  selector: 'app-title',
  templateUrl: './title.component.html',
  styleUrls: ['./title.component.scss']
})
export class TitleComponent implements OnInit {
 @Input() title: string;
 @Input() iconClass = 'fa fa-user';
 @Input() subtitle = 'A melhor plataforma de eventos';
 @Input() btnList = false;
 @Input() routerRef: string;

  constructor( private router: Router) { }

  ngOnInit(): void {
  }

  listEvents(): void {
    this.router.navigate([`/${this.routerRef}/list`]);
  }
}

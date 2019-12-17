import { WorkItemService } from './../../shared/work-item/work-item.service';
import { ActivatedRoute } from '@angular/router';
import { WorkItem } from './../../shared/work-item/work-item.model';
import { Component, OnInit } from '@angular/core';

@Component({
  selector: 'app-work-item-detail',
  templateUrl: './work-item-detail.component.html',
  styles: []
})
export class WorkItemDetailComponent implements OnInit {
  workItem: WorkItem;
  constructor(private workItemService: WorkItemService, private activatedRoute: ActivatedRoute) { }

  ngOnInit() {
    let entityId;
    this.activatedRoute.paramMap.subscribe(params => {
      entityId = +params.get('id');
    });
    this.workItemService.loadEntity(entityId).subscribe(
      res => {
        this.workItem = res as WorkItem;
      },
      err => {
        console.log(err);
      }
    );
  }
}

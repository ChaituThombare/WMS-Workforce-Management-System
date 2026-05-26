import { Component, inject, OnInit, ChangeDetectorRef } from '@angular/core';
import { AnnouncementService } from '../../../core/services/announcement.service';
import { AuthService } from '../../../core/services/auth.service';
import { Announcement } from '../../../core/models/announcement';

@Component({
  selector: 'app-announcements',
  standalone: false,
  templateUrl: './announcements.html',
  styleUrl: './announcements.css',
})
export class Announcements implements OnInit {
  private announcementService = inject(AnnouncementService);
  private authService = inject(AuthService);
  private cdr = inject(ChangeDetectorRef);

  announcements: Announcement[] = [];

  announcementData = {
    title: '',
    message: '',
    createdBy: localStorage.getItem('fullName') || 'Admin'
  };

  ngOnInit(): void {
    this.loadAnnouncements();
  }

  loadAnnouncements(): void {
    const request = this.isAdmin()
      ? this.announcementService.getAll()
      : this.announcementService.getActive();

    request.subscribe({
      next: (data) => {
        this.announcements = [...data];
        this.cdr.detectChanges();
      }
    });
  }

  createAnnouncement(): void {
    if (
      !this.announcementData.title.trim() ||
      !this.announcementData.message.trim()
    ) {
      alert('Title and message are required');
      return;
    }
    
    this.announcementService.create(this.announcementData).subscribe({
      next: () => {
        alert('Announcement created successfully');

        this.announcementData = {
          title: '',
          message: '',
          createdBy: localStorage.getItem('fullName') || 'Admin'
        };

        this.loadAnnouncements();
      }
    });
  }

  toggleStatus(id: number): void {
    this.announcementService.toggleStatus(id).subscribe({
      next: () => {
        this.loadAnnouncements();
      }
    });
  }

  isAdmin(): boolean {
    return this.authService.isAdmin();
  }
}
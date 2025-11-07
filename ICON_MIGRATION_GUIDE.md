# Font Awesome 6 Icon Migration Guide

## Complete Icon Mapping Reference

### ✅ Already Migrated
- Dashboard (Index.cshtml) - Complete
- Navigation (_NavPartial.cshtml) - Complete
- Footer (_FooterPartial.cshtml) - Complete
- Header (HeaderPartial.cshtml) - Complete
- Login (Account/Login.cshtml) - Complete

### 🔄 Icon Replacement Map (Feather → Font Awesome 6)

| Feather Icon | Font Awesome 6 Class | Usage Context |
|--------------|---------------------|---------------|
| `data-feather="user"` | `fa-solid fa-user` | User profile, forms |
| `data-feather="users"` | `fa-solid fa-users` | Multiple users, groups |
| `data-feather="user-plus"` | `fa-solid fa-user-plus` | Add user/trainee |
| `data-feather="user-check"` | `fa-solid fa-user-check` | Verified user |
| `data-feather="user-graduate"` | `fa-solid fa-user-graduate` | Trainees |
| `data-feather="lock"` | `fa-solid fa-lock` | Password fields |
| `data-feather="eye"` | `fa-solid fa-eye` | Show password |
| `data-feather="eye-off"` | `fa-solid fa-eye-slash` | Hide password |
| `data-feather="mail"` | `fa-solid fa-envelope` | Email |
| `data-feather="book"` | `fa-solid fa-book` | Single book/course |
| `data-feather="book-open"` | `fa-solid fa-book-open` | Open book/courses |
| `data-feather="award"` | `fa-solid fa-award` | Grades/achievements |
| `data-feather="building"` | `fa-solid fa-building` | Department/building |
| `data-feather="map-pin"` | `fa-solid fa-location-dot` | Location/address |
| `data-feather="upload"` | `fa-solid fa-upload` | File upload |
| `data-feather="save"` | `fa-solid fa-floppy-disk` | Save action |
| `data-feather="edit"` | `fa-solid fa-pen` | Edit action |
| `data-feather="edit-2"` | `fa-solid fa-pen-to-square` | Edit with context |
| `data-feather="trash-2"` | `fa-solid fa-trash-can` | Delete action |
| `data-feather="chevron-down"` | `fa-solid fa-chevron-down` | Dropdown arrow |
| `data-feather="check-circle"` | `fa-solid fa-circle-check` | Success/complete |
| `data-feather="alert-triangle"` | `fa-solid fa-triangle-exclamation` | Warning/error |
| `data-feather="alert-circle"` | `fa-solid fa-circle-exclamation` | Alert |
| `data-feather="info"` | `fa-solid fa-circle-info` | Information |
| `data-feather="home"` | `fa-solid fa-house` | Home page |
| `data-feather="arrow-right"` | `fa-solid fa-arrow-right` | Navigation forward |
| `data-feather="arrow-left"` | `fa-solid fa-arrow-left` | Navigation back |
| `data-feather="calendar"` | `fa-solid fa-calendar` | Date/calendar |
| `data-feather="inbox"` | `fa-solid fa-inbox` | Empty state |
| `data-feather="compass"` | `fa-solid fa-compass` | Explore/navigate |
| `data-feather="camera"` | `fa-solid fa-camera` | Photo/image |
| `data-feather="dollar-sign"` | `fa-solid fa-dollar-sign` | Money/salary |
| `data-feather="list"` | `fa-solid fa-list` | List view |
| `data-feather="refresh-cw"` | `fa-solid fa-arrows-rotate` | Refresh/reload |
| `data-feather="help-circle"` | `fa-solid fa-circle-question` | Help/question |
| `data-feather="hash"` | `fa-solid fa-hashtag` | ID/reference |
| `data-feather="graduation-cap"` | `fa-solid fa-graduation-cap` | Education/student |
| `data-feather="briefcase"` | `fa-solid fa-briefcase` | Department/work |
| `data-feather="shield"` | `fa-solid fa-shield-halved` | Privacy/security |
| `data-feather="heart"` | `fa-solid fa-heart` | Like/favorite |
| `data-feather="message-circle"` | `fa-solid fa-message` | Chat/messaging |
| `data-feather="layers"` | `fa-solid fa-layer-group` | Brand/layers |
| `data-feather="log-in"` | `fa-solid fa-right-to-bracket` | Login |
| `data-feather="log-out"` | `fa-solid fa-right-from-bracket` | Logout |
| `data-feather="activity"` | `fa-solid fa-chart-line` | Dashboard/activity |
| `data-feather="trending-up"` | `fa-solid fa-chart-simple` | Statistics/trends |
| `data-feather="pie-chart"` | `fa-solid fa-chart-pie` | Pie chart |
| `data-feather="folder"` | `fa-solid fa-folder` | Folder/category |
| `data-feather="plus-circle"` | `fa-solid fa-circle-plus` | Add/create |

## Files Requiring Updates

### High Priority (User-facing)
1. ✅ Views/Account/Login.cshtml
2. Views/Account/Register.cshtml
3. Views/Account/RegisterAdmin.cshtml
4. Views/Home/Index.cshtml
5. Views/Home/Privacy.cshtml
6. Views/Profile/Index.cshtml

### Medium Priority (Admin/Management)
7. Views/Trainee/Index.cshtml
8. Views/Trainee/Edit.cshtml
9. Views/Trainee/AddTrainee.cshtml
10. Views/Instructor/Index.cshtml
11. Views/Instructor/Edit.cshtml
12. Views/Instructor/addInstructor.cshtml

### Course Management
13. Views/Course/Index.cshtml
14. Views/Course/Edit.cshtml
15. Views/Course/AddCourse.cshtml
16. Views/Course/ViewCourseDetails.cshtml
17. Views/Course/CourseDetailsPartial.cshtml

### Partials & Support
18. Views/Shared/Error.cshtml
19. Views/Instructor/_ShowSuccessPartial.cshtml
20. Views/Instructor/ShowCoursesPerDeptPartial.cshtml
21. Views/Course/_EnrollSuccessPartial.cshtml

## JavaScript Updates Needed

Remove all instances of:
```javascript
feather.replace();
```

## CSS Updates

✅ Added comprehensive Font Awesome alignment CSS in `/wwwroot/css/fontawesome-fixes.css`

Key features:
- Vertical alignment for all icon contexts
- Consistent sizing (sm, md, lg, xl)
- Flexbox alignment for icons with text
- Proper spacing management
- Color inheritance
- Hover effect preservation

## Verification Checklist

- [x] Font Awesome 6.5.0 CDN loaded in HeaderPartial
- [x] fontawesome-fixes.css created and linked
- [x] Dashboard icons migrated
- [x] Navigation icons migrated
- [x] Footer icons migrated
- [ ] All form pages (Register, Add/Edit)
- [ ] All index/list pages
- [ ] All detail/view pages
- [ ] All partial views
- [ ] Remove all feather.replace() calls
- [ ] Remove Feather Icons CDN reference
- [ ] Test icon alignment on all browsers
- [ ] Test icon colors match theme (black/white)
- [ ] Verify hover effects work
- [ ] Check mobile responsiveness

## Notes
- All icons use black/white/neutral colors per site theme
- Icons are vertically centered with adjacent text
- Spacing maintained with Bootstrap utility classes (me-2, ms-1, etc.)
- No JavaScript required for Font Awesome icons
- Better performance vs Feather (no JS parsing)

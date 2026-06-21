import { NavLink } from 'react-router-dom';

export default function Navbar() {
  return (
    <nav className="navbar">
      <div className="navbar-brand">
        <span className="navbar-icon">🎵</span>
        <span className="navbar-title">BeatFlow</span>
      </div>
      <div className="navbar-links">
        <NavLink to="/" className={({ isActive }) => isActive ? 'nav-link active' : 'nav-link'} end>
          Dashboard
        </NavLink>
        <NavLink to="/genres" className={({ isActive }) => isActive ? 'nav-link active' : 'nav-link'}>
          Gêneros
        </NavLink>
        <NavLink to="/artists" className={({ isActive }) => isActive ? 'nav-link active' : 'nav-link'}>
          Artistas
        </NavLink>
        <NavLink to="/tracks" className={({ isActive }) => isActive ? 'nav-link active' : 'nav-link'}>
          Músicas
        </NavLink>
        <NavLink to="/playlists" className={({ isActive }) => isActive ? 'nav-link active' : 'nav-link'}>
          Playlists
        </NavLink>
      </div>
    </nav>
  );
}
